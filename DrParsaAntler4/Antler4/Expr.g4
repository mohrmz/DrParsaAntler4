grammar Expr;		

a : Id ':=' e ;
e : e '+' t | e '-' t | t ;
t : t '*' f | t '/' f | f ;
f : Id | No | '(' e ')' ;
Id : LETTER(LETTER|DIGIT|'_')* ;
No : DIGIT+ ; 
fragment LETTER : [a-z,A-Z] ;
fragment DIGIT : [0-9]  ;



