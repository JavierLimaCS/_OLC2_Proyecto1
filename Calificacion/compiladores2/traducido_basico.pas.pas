program basico;

    const
        v  = true;
        f  = false;
        datos = 3;

    var
        val1 : integer = 0;
        val2 : integer = 0;
        val3 : integer = 0;
        resp : integer = 0;
        a : integer = 0;
        b : integer = 0;

procedure imprimirvalores();
begin
    writeln('-----------------------');
    write('el valor de v es: ');
    writeln(v);
    write('el valor de f es: ');
    writeln(f);
    write('el valor de datos es: ');
    writeln(datos);
    write('el valor de val1 es: ');
    writeln(val1);
    write('el valor de val2 es: ');
    writeln(val2);
    write('el valor de val3 es: ');
    writeln(val3);
    write('el valor de resp es: ');
    writeln(resp);
    write('el valor de a es: ');
    writeln(a);
    write('el valor de b es: ');
    writeln(b);
    writeln('-----------------------');
end;

function sumarnumeros(num1, num2 : integer):integer;
var
	resp : integer;
begin
    resp := num1 + num2;
    write('el resultado de tu suma es: ');
    writeln(resp);
    exit(resp);
end;

procedure iniciarvalores();
begin
    writeln('----dentro de iniciar valores----');
    
    val1 := 7 - (5 + 10 * (2 + 4 * (5 + 2 * 3)) - 8 * 3 * 3) + 50 * (6 * 2);
    val2 := (2 * 2 * 2 * 2) - 9 - (8 - 6 + (3 * 3 - 6 * 5 - 7 - (9 + 7 * 7 * 7) + 10) - 5) + 8 - (6 - 5 * (2 * 3));
    val3 := val1 + ((2 + val2 * 3) + 1 - ((2 * 2 * 2) - 2) * 2) - 2;

    a := val1 + val2 - val3 + sumarnumeros(5, val1);
    b := sumarnumeros(5, a) - val1 * 2;

    resp := val1 + val2 + sumarnumeros(val3, resp);

    imprimirvalores();

    writeln('-----------------------');
end;

function decisiones() : boolean;
var
    valorverdadero : integer = 100;
begin
    writeln('----dentro de decisiones----');
    if((valorverdadero = (50 + 50 + (val1 - val1))) and not not not not not not not not not false) then
    begin
        writeln('en este lugar deberia de entrar :)');
        valorverdadero := 50;
    end
    else if (f or (valorverdadero > 50)) and ((resp <> 100) and not not not not not v) then
    begin
        writeln('aca no deberia de entrar :ccc');
        valorverdadero := 70;
    end
    else
    begin
        writeln('aca no deberia de entrar :cccc');
    end;

    case valorverdadero of
        70:
        begin
            writeln('no deberia entrar :p');
        end;
        50:
        begin
            writeln('entro!? que bueno :d');
            writeln('-----------------------');
            exit(true);
            writeln('no deberia imprimir esto o:');
        end;
        100:
        begin
            writeln('no deberia entrar :p');
        end;
        else
        begin
            writeln('no deberia entrar :p');
        end;
    end;

    writeln('-----------------------');
    exit(false and true);
end;

procedure ciclosycontrol();
var
    i : integer = 0;
begin
    writeln('----dentro de ciclos y control----');

    writeln('while');
    while i < datos do
    begin
        write('el valor de i: ');
        writeln(i);
        i := i + 1;
        continue;
        writeln('esto no deberia imprimir dentro de while');
    end;

    writeln('for do');
    for i := 0 to 10 do
    begin
        if i = 8 then
        begin
            break;
        end;

        write('el valor de i: ');
        writeln(i);
    end;

    writeln('repeat');
    i := 6;
    repeat
	begin
        write('el valor de i: ');
        writeln(i);
        i := i - 2;
	end
    until (i = 0);

    writeln('-----------------------');
end;

procedure inicio();
begin
    writeln('----------------------');
    writeln('----archivo basico----');
    writeln('----------------------');

    imprimirvalores();

    iniciarvalores();

    writeln('dentro de inicio');
    writeln(sumarnumeros(5, 5));
	{*funciona bien hasta aca*}
    if(decisiones()) then
    begin
        writeln('esto deberia de imprimirse...');
    end
    else
    begin
        writeln('no deberia entrar aca :d');
    end;

    ciclosycontrol();

    writeln('----------------------------------------');
    writeln('----esperemos que haya funcionado :d----');
    writeln('----------------------------------------');
end;

begin
    inicio();
end.

{
----------------------                                                                                                      
----archivo basico----                                                                                                      
----------------------                                                                                                      
-----------------------                                                                                                     
el valor de v es: true                                                                                                      
el valor de f es: false                                                                                                     
el valor de datos es: 3                                                                                                     
el valor de val1 es: 0                                                                                                      
el valor de val2 es: 0                                                                                                      
el valor de val3 es: 0                                                                                                      
el valor de resp es: 0                                                                                                      
el valor de a es: 0                                                                                                         
el valor de b es: 0                                                                                                         
-----------------------                                                                                                     
----dentro de iniciar valores----                                                                                           
el resultado de tu suma es: 219                                                                                             
el resultado de tu suma es: -19959                                                                                           
el resultado de tu suma es: -19753                                                                                          
-----------------------                                                                                                     
el valor de v es: true                                                                                                      
el valor de f es: false                                                                                                     
el valor de datos es: 3                                                                                                     
el valor de val1 es: 214
el valor de val2 es: -2781                                                                                                  
el valor de val3 es: -17858                                                                                                 
el valor de resp es: -20425                                                                                                 
el valor de a es: 15510                                                                                                     
el valor de b es: 15087                                                                                                     
-----------------------                                                                                                     
-----------------------                                                                                                     
dentro de inicio                                                                                                            
el resultado de tu suma es: 10                                                                                              
10                                                                                                                          
----dentro de decisiones----                                                                                                
en este lugar deberia de entrar :)                                                                                          
entro!? que bueno :d                                                                                                        
-----------------------                                                                                                     
esto deberia de imprimirse...                                                                                               
----dentro de ciclos y control----                                                                                          
while                                                                                                                       
el valor de i: 0                                                                                                            
el valor de i: 1                                                                                                            
el valor de i: 2                                                                                                            
for do                                                                                                                      
el valor de i: 0                                                                                                            
el valor de i: 1
el valor de i: 2                                                                                                            
el valor de i: 3                                                                                                            
el valor de i: 4                                                                                                            
el valor de i: 5                                                                                                            
el valor de i: 6                                                                                                            
el valor de i: 7                                                                                                            
repeat                                                                                                                      
el valor de i: 6                                                                                                            
el valor de i: 4                                                                                                            
el valor de i: 2                                                                                                            
-----------------------                                                                                                     
----------------------------------------                                                                                    
----esperemos que haya funcionado :d----                                                                                    
----------------------------------------
}
