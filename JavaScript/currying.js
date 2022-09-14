
function curry(uncurried) {
  const requireLength = uncurried.length;
  const slice = Array.prototype.slice;
  
  return (function resolver(){
    let storedArgs = slice.call(arguments); // 전달된 arguments를 기억하고 있음
    
    return function() {
      let newArgs = storedArgs.slice(); // 이전에 추가된 arguments를 복사
      Array.prototype.push.apply(newArgs, arguments); // 새로 추가된 arguments를 기존의 arguments에 복사
      next = newArgs.length >= requireLength ? uncurried: resolver;
      
      return next.apply(null, newArgs); // localArgs를 arguments에 넣어 호출
    }
  }());
}

function sum(x,y,z) {
 return x + y + z; 
}

const curriedSum = curry(sum);
console.log(curriedSum(1)(2)(3)); // 6
