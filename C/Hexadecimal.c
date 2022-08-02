#include <stdio.h>

int main(){
  int test = 0x12345678;
  char *pt = (char*)&test; // a pointer that points only one byte
  
  for(int i=0; i<sizeof(int); i++){
    printf("%x", pt[i]); 
  }
  
  return 0;
}
