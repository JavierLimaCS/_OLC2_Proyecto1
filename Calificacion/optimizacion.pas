program Optimizacion;
var a : integer = 100;
    b : integer = 200;
    c : integer = 300;

procedure condicionales();
begin 
	if (100=100) then writeln('Regla 3');

	if (250<200) then writeln('No deberia imprimirese')
        else writeln('Regla 4');
end;

procedure comunes(n1, n2, n3 :integer);
begin
	write('valor inicial de n1:');
	writeln(n1);
	write('valor inicial de n2:');
	writeln(n2);
	write('valor inicial de n3:');
	writeln(n3);
	n1 := n1+0;
	n2 := 0/n1;
	write('valor inicial de n1:');
	writeln(n1);
	write('valor inicial de n2:');
	writeln(n2);
	write('valor inicial de n3:');
	writeln(n3);
end;

begin 
condicionales();
comunes(a,b,c);
end.