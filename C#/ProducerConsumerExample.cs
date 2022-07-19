using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace CSBlockingCollection
{
	public class MyProduceConsumeQueue
	{
		// 추가/추출된 데이터 개수
		private int _addCount = 0;
		private int _takeCount = 0;

		// 블로킹 컬렉션 (내부적으론 ConcurrentQueue 사용)
		private BlockingCollection<int> _queue = new BlockingCollection<int>();

		// 최대 사이즈 제한을 걸수도 있다. 사이즈 제한이 있는 경우 큐가 가득 차면, 생산자가 대기해야 한다.
		//private BlockingCollection<int> _queue = new BlockingCollection<int>(10);

		// 추출 취소를 위한 토큰
		private CancellationTokenSource _source = new CancellationTokenSource();


		// 락을 위한 오브젝트
		public object LockObj = new object();

		// 큐 사이즈
		public int Count { get { return _queue.Count; } }

		// 추가된 데이터 개수
		public int AddedCount { get { return _addCount; } }

		// 추출한 데이터 개수
		public int TakenCount { get { return _takeCount; } }
		
		// 데이터 추가 중단
		public void CompleteAdding() { _queue.CompleteAdding(); }

		// 데이터 큐가 완전히 비었는지 검사
		public bool IsCompleted() { return _queue.IsCompleted; }

		// 데이터 추출 중단
		public void CancelTake() { _source.Cancel(); }


		// 데이터 추가
		public bool Add(int data)
		{
			try
			{
				_queue.Add(data);

				//++_addCount;
				Interlocked.Increment(ref _addCount);

				return true;
			}
			catch (Exception e)
			{
				// CompleteAdding를 호출하면 여기서 예외가 발생한다.
				Console.WriteLine(e.Message);
				return false;
			}
		}

		// 데이터 추출
		public bool Take(ref int data)
		{
			try
			{
				data = _queue.Take(_source.Token);

				//++_takeCount;
				Interlocked.Increment(ref _takeCount);

				return true;
			}
			catch (Exception e)
			{
				// CancelTake를 호출하면 여기서 예외가 발생한다.
				Console.WriteLine(e.Message);
				return false;
			}
		}

		// 큐 내용 출력하기 - 디버깅용
		public void PrintContents()
		{
			Console.Write($"[Queue] Add({AddedCount}), Take({TakenCount}), Count({_queue.Count}) => ");

			foreach (int item in _queue)
			{
				Console.Write("{0} ", item);
			}

			Console.WriteLine("");
		}
	}

	// 생산자 소비자 공통 부모용 클래스
	public abstract class ProducerConsumerBase
	{
		// 데이터 하나 처리에 필요한 최소, 최대 시간 시뮬레이션 값 (ms 단위)
		int _minProcessTime;
		int _maxProcessTime;

		// 랜덤
		protected Random _random = new Random();

		// 데이터 전달용 큐
		protected MyProduceConsumeQueue _queue;

		// 스레드 아이디
		public int ThreadId { get; private set; }

		// 데이터 처리된 개수
		public int ProcessedCount { get; set; }


		// 생성자
		public ProducerConsumerBase(MyProduceConsumeQueue q, int minProcessTime, int maxProcessTime)
		{
			_queue = q;

			_minProcessTime = minProcessTime;
			_maxProcessTime = maxProcessTime;
		}

		// 스레드 시작시 호출
		protected void OnThreadStart()
		{
			ThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		// 스레드 잠시 대기 (데이터 처리 시뮬레이션용)
		protected void ThreadWait()
		{
			Thread.Sleep(_random.Next(_minProcessTime, _maxProcessTime));
		}

		// 스레드 함수
		public abstract void ThreadRun();
	}


	// 생산자
	public class Producer : ProducerConsumerBase
	{
		// 생성자
		public Producer(MyProduceConsumeQueue q, int minProcessTime, int maxProcessTime)
			: base(q, minProcessTime, maxProcessTime)
		{}

		// 스레드 함수
		public override void ThreadRun()
		{
			// 스레드 시작 처리
			OnThreadStart();

			//Stopwatch stopwatch = new Stopwatch();

			while(true)
			{
				// 랜덤 데이터 생성
				int data = _random.Next(0, 100);

				//stopwatch.Restart();

				// 데이터를 큐에 추가. 스레드에 안전하다.
				if (_queue.Add(data) == false)
					break;

				//stopwatch.Stop();

				// 대기시간이 있을 경우 표시
				//if (stopwatch.ElapsedMilliseconds != 0)
					//Console.WriteLine($"[{ThreadId:D2}] Produce Add Time : {stopwatch.ElapsedMilliseconds} ms");

				// 처리 카운트 증가
				++ProcessedCount;

				// 디버깅용 콘솔 출력 부분 (락을 걸어야 콘솔 출력이 깨지지 않는다.)
				// Add 와 별도로 락을 걸었기 때문에 출력되는 큐의 내용은 정확하지 않을 수 있다.
				lock (_queue.LockObj)
				{
					Console.Write($"[{ThreadId:D2}] Produce ({data:D2}) => ");
					_queue.PrintContents();
				}

				// 스레드 잠시 대기
				ThreadWait();
			}

			// 생산자 결과 출력
			Console.WriteLine($"[{ThreadId:D2}] Produced {ProcessedCount} items");
		}
	}

	// 소비자
	public class Consumer : ProducerConsumerBase
	{
		// 생성자
		public Consumer(MyProduceConsumeQueue q, int minProcessTime, int maxProcessTime)
			: base(q, minProcessTime, maxProcessTime)
		{ }

		// 스레드 함수
		public override void ThreadRun()
		{
			// 스레드 시작 처리
			OnThreadStart();

			while (true)
			{
				int data = 0;

				// 큐에서 데이터 하나 추출. 스레드에 안전하다.
				if (_queue.Take(ref data) == false)
					break;

				// 처리 카운트 증가
				++ProcessedCount;

				// 디버깅용 콘솔 출력 부분 (락을 걸어야 콘솔 출력이 깨지지 않는다.)
				// Take 와 별도로 락을 걸었기 때문에 출력되는 큐의 내용은 정확하지 않을 수 있다.
				lock (_queue.LockObj)
				{
					Console.Write($"[{ThreadId:D2}] Consume ({data:D2}) => ");
					_queue.PrintContents();
				}

				// 스레드 잠시 대기
				ThreadWait();
			}

			// 소비자 결과 출력
			Console.WriteLine($"[{ThreadId:D2}] Consumed {ProcessedCount} items", ProcessedCount);
		}
	}

	// 샘플 클래스
	public class ThreadSyncSample
	{
		static void Main()
		{
			Console.WriteLine("Configuring worker thread...");
			
			MyProduceConsumeQueue queue = new MyProduceConsumeQueue();

			// 생산자 정의
			Producer[] producerList = new[]
			{
				new Producer(queue, 100, 300),
				new Producer(queue, 200, 400),
			};

			// 소비자 정의 - 처리 속도를 느리게 하면 큐가 점점 쌓인다.
			Consumer[] consumerList = new[]
			{
				//new Consumer(queue, 100, 300),
				//new Consumer(queue, 100, 300),
				new Consumer(queue, 100, 300),				
				new Consumer(queue, 200, 600),
			};

			// 생산자 소비자 태스크 정의
			var producerTasks = new Task[producerList.Length];
			var consumerTasks = new Task[consumerList.Length];

			// 생산자 생성
			for (int i = 0; i < producerTasks.Length; ++i)
				producerTasks[i] = new Task(producerList[i].ThreadRun);

			// 소비자 생성
			for (int i = 0; i < consumerTasks.Length; ++i)
				consumerTasks[i] = new Task(consumerList[i].ThreadRun);


			Console.WriteLine("Launching producer and consumer threads...");
			
			// 생산자 태스크 실행
			Array.ForEach(producerTasks, t => t.Start());

			// 소비자 태스크 실행
			Array.ForEach(consumerTasks, t => t.Start());


			// ESC 키를 누르면 생산자를 중단한다는 안내
			Console.WriteLine("Press ESC to stop producers");

			// 키 입력이 있는가?
			while (true)
			{
				if (Console.KeyAvailable)
				{
					// 입력된 키가 ESC인가?
					var keyInfo = Console.ReadKey();
					if (keyInfo.Key == ConsoleKey.Escape)
					{
						// 생산자 스레드 중단 요청
						Console.WriteLine("Signaling producer threads to terminate...");
						queue.CompleteAdding();
						break;
					}
				}
			}

			// 생산자 스레드 종료 대기
			Task.WaitAll(producerTasks);


			// ESC 키를 누르면 소비자를 중단한다는 안내
			Console.WriteLine("Press ESC to stop consumers");

			// 큐가 빌 때까지 계속 대기
			while (queue.IsCompleted() == false)
			{
				if (Console.KeyAvailable)
				{
					// 입력된 키가 ESC인가?
					var keyInfo = Console.ReadKey();
					if (keyInfo.Key == ConsoleKey.Escape)
					{
						// 소비자 스레드 중단 요청
						Console.WriteLine("Signaling consumer threads to terminate...");
						queue.CancelTake();
						break; // while 빠져나가기
					}
				}
			}

			// 소비자 스레드 종료 대기
			Task.WaitAll(consumerTasks);

			Console.WriteLine("========================================");

			// 전체 생산량 계산
			int totalProduced = 0;
			foreach (var item in producerList)
			{
				Console.WriteLine($"[{item.ThreadId:D2}] Produced count : {item.ProcessedCount}");
				totalProduced += item.ProcessedCount;
			}

			// 전체 소비량 계산
			int totalConsumed = 0;
			foreach (var item in consumerList)
			{
				Console.WriteLine($"[{item.ThreadId:D2}] Consumed count : {item.ProcessedCount}");
				totalConsumed += item.ProcessedCount;
			}

			// 결과 출력
			Console.WriteLine($"Total Produced count : {totalProduced}");
			Console.WriteLine($"Total Consumed count : {totalConsumed}");
			Console.WriteLine($"Queue count : {queue.Count}");
			Console.WriteLine($"Queue add count : {queue.AddedCount}");
			Console.WriteLine($"Queue take count : {queue.TakenCount}");

			// 결과 검증 코드
			if (queue.AddedCount != totalProduced)
				Console.WriteLine($"ERROR : _queue.AddCount != totalProduced");

			if (queue.TakenCount != totalConsumed)
				Console.WriteLine($"ERROR : _queue.TakeCount != totalConsumed");

			if (queue.Count != (totalProduced - totalConsumed))
				Console.WriteLine($"ERROR : _queue.Count != (totalProduced - totalConsumed)");

			// 종료 대기
			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();
		}
	}
}
