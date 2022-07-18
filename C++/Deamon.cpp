#include <signal.h>
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <sys/types.h>
const int MAXFD = 64;


void daemon_init()
{
  pid_t pid;
  if ((pid = fork()) != 0) 
      exit(0);

  setsid();
  signal(SIGHUP, SIG_IGN);

  chdir("/");

  umask(0);
  for (int i=0; i<MAXFD; i++) 
      close(i);
}


int main(int argc, char const* argv[])
{
  daemon_init2();
  for (;;)
    sleep(1);

  return 0;
}
