grammar MGPL;
options { caseInsensitive = true; }

chunk
    : block EOF
    ;

block
    : stat* laststat?
    ;

stat
    : varlist assignment explist ';'
    | functioncall ';'
    | label ';'
    | 'break' ';'
    | 'goto' NAME ';'
    | 'do' block 'end'
    | 'while' exp 'do' block 'end'
    // | 'if' '(' exp ')' 'begin' block ('else' 'if' '(' exp ')' 'begin' block)* ('else' block)? 'end'
    | if_stat
    | 'for' NAME '=' exp ',' exp (',' exp)? 'do' block 'end'
    | 'for' namelist 'in' explist 'do' block 'end'
    | func_dec
    ;

assignment
    : operatorAddSub? '='
    ;

if_stat
    : 'if' '(' exp ')' stat_or_block ('else' stat_or_block)?
    ;

stat_or_block
    : 'begin' block 'end'
    | stat
    ;
    
func_dec
    : 'function' NAME '(' parlist? ')' func_variables func_body
    ;

func_variables
    : 'declare' func_var_dec*
    ;

func_var_dec
    : type NAME (',' NAME)* ';'
    ;
    
func_body
    : 'begin' block 'end'
    ;
    
type
    : 'agent'
    | 'list'
    | 'integer'
    ;

attnamelist
    : NAME attrib (',' NAME attrib)*
    ;

attrib
    : ('<' NAME '>')?
    ;

laststat
    : 'return' explist? | 'break' | 'continue' ';'?
    ;

label
    : '::' NAME '::'
    ;

funcname
    : NAME ('.' NAME)* (':' NAME)?
    ;

varlist
    : var (',' var)*
    ;

namelist
    : NAME (',' NAME)*
    ;

explist
    : (exp ',')* exp
    ;

exp
    : 'nil' | 'false' | 'true'
    | number
    | string
    | '...'
    | functiondef
    | prefixexp
    | tableconstructor
    | <assoc=right> exp operatorPower exp
    | operatorUnary exp
    | exp operatorMulDivMod exp
    | exp operatorAddSub exp
    | <assoc=right> exp operatorStrcat exp
    | exp operatorComparison exp
    | exp operatorAnd exp
    | exp operatorOr exp
    | exp operatorBitwise exp
    | NAME propertyAccessor
    ;
	
propertyAccessor
	: '\'s' string
	;


prefixexp
    : varOrExp nameAndArgs*
    ;

functioncall
    : varOrExp nameAndArgs+
    ;

varOrExp
    : var | '(' exp ')'
    ;

var
    : (NAME | '(' exp ')' varSuffix) varSuffix*
    ;

varSuffix
    : ('[' exp ']' | propertyAccessor)
    ;

nameAndArgs
    : (':' NAME)? args
    ;

args
    : '(' explist? ')' | tableconstructor | string
    ;

functiondef
    : 'function' funcbody
    ;

funcbody
    : '(' parlist? ')' block 'end'
    ;

parlist
    : namelist (',' '...')? | '...'
    ;

tableconstructor
    : '{' fieldlist? '}'
    ;

fieldlist
    : field (fieldsep field)* fieldsep?
    ;

field
    : '[' exp ']' '=' exp | NAME '=' exp | exp
    ;

fieldsep
    : ',' | ';'
    ;

operatorOr
	: '||';

operatorAnd
	: '&&';

operatorComparison
	: '<' | '>' | '<=' | '>=' | '~=' | '==';

operatorStrcat
	: '..';

operatorAddSub
	: '+' | '-';

operatorMulDivMod
	: '*' | '/' | '%' | '//';

operatorBitwise
	: '&' | '|' | '~' | '<<' | '>>';

operatorUnary
    : 'not' | '#' | '-' | '~';

operatorPower
    : '^';

number
    : INT | HEX | FLOAT | HEX_FLOAT
    ;

string
    : NORMALSTRING | LONGSTRING
    ;

// LEXER

NAME
    : [$#a-z_][a-z_0-9]*
    ;

NORMALSTRING
    : '"' ( EscapeSequence | ~('\\'|'"') )* '"'
    ;

//CHARSTRING
//    : '\'' ( EscapeSequence | ~('\''|'\\') )* '\''
//    ;

LONGSTRING
    : '[' NESTED_STR ']'
    ;

fragment
NESTED_STR
    : '=' NESTED_STR '='
    | '[' .*? ']'
    ;

INT
    : Digit+
    ;

HEX
    : '0' [x] HexDigit+
    ;

FLOAT
    : Digit+ '.' Digit* ExponentPart?
    | '.' Digit+ ExponentPart?
    | Digit+ ExponentPart
    ;

HEX_FLOAT
    : '0' [x] HexDigit+ '.' HexDigit* HexExponentPart?
    | '0' [x] '.' HexDigit+ HexExponentPart?
    | '0' [x] HexDigit+ HexExponentPart
    ;

fragment
ExponentPart
    : [e] [+-]? Digit+
    ;

fragment
HexExponentPart
    : [p] [+-]? Digit+
    ;

fragment
EscapeSequence
    : '\\' [abfnrtvz"'|$#\\]   // World of Warcraft Lua additionally escapes |$# 
    | '\\' '\r'? '\n'
    | DecimalEscape
    | HexEscape
    | UtfEscape
    ;

fragment
DecimalEscape
    : '\\' Digit
    | '\\' Digit Digit
    | '\\' [0-2] Digit Digit
    ;

fragment
HexEscape
    : '\\' 'x' HexDigit HexDigit
    ;

fragment
UtfEscape
    : '\\' 'u{' HexDigit+ '}'
    ;

fragment
Digit
    : [0-9]
    ;

fragment
HexDigit
    : [0-9a-f]
    ;

fragment
SingleLineInputCharacter
    : ~[\r\n\u0085\u2028\u2029]
    ;

COMMENT
    : '--[' NESTED_STR ']' -> channel(HIDDEN)
    ;

LINE_COMMENT
    : '//' SingleLineInputCharacter* -> channel(2)
    ;

WS
    : [ \t\u000C\r\n]+ -> skip
    ;

SHEBANG
    : '!' SingleLineInputCharacter* -> channel(HIDDEN)
    ;
