program hello;

type 
numero = array [1..10] of integer;

var listanumeros : numero;

begin
  listanumeros[1]:= 21;
  writeln(listanumeros[1]);
end.
