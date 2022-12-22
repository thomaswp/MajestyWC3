//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.11.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from MGPL.g4 by ANTLR 4.11.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="MGPLParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.11.1")]
[System.CLSCompliant(false)]
public interface IMGPLListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.chunk"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterChunk([NotNull] MGPLParser.ChunkContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.chunk"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitChunk([NotNull] MGPLParser.ChunkContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlock([NotNull] MGPLParser.BlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlock([NotNull] MGPLParser.BlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStat([NotNull] MGPLParser.StatContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStat([NotNull] MGPLParser.StatContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignment([NotNull] MGPLParser.AssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignment([NotNull] MGPLParser.AssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.if_stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIf_stat([NotNull] MGPLParser.If_statContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.if_stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIf_stat([NotNull] MGPLParser.If_statContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.stat_or_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStat_or_block([NotNull] MGPLParser.Stat_or_blockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.stat_or_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStat_or_block([NotNull] MGPLParser.Stat_or_blockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.func_dec"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunc_dec([NotNull] MGPLParser.Func_decContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.func_dec"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunc_dec([NotNull] MGPLParser.Func_decContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.func_variables"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunc_variables([NotNull] MGPLParser.Func_variablesContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.func_variables"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunc_variables([NotNull] MGPLParser.Func_variablesContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.func_var_dec"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunc_var_dec([NotNull] MGPLParser.Func_var_decContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.func_var_dec"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunc_var_dec([NotNull] MGPLParser.Func_var_decContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.func_body"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunc_body([NotNull] MGPLParser.Func_bodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.func_body"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunc_body([NotNull] MGPLParser.Func_bodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType([NotNull] MGPLParser.TypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType([NotNull] MGPLParser.TypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.attnamelist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAttnamelist([NotNull] MGPLParser.AttnamelistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.attnamelist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAttnamelist([NotNull] MGPLParser.AttnamelistContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.attrib"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAttrib([NotNull] MGPLParser.AttribContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.attrib"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAttrib([NotNull] MGPLParser.AttribContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.laststat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLaststat([NotNull] MGPLParser.LaststatContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.laststat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLaststat([NotNull] MGPLParser.LaststatContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLabel([NotNull] MGPLParser.LabelContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLabel([NotNull] MGPLParser.LabelContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.funcname"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFuncname([NotNull] MGPLParser.FuncnameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.funcname"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFuncname([NotNull] MGPLParser.FuncnameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.varlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarlist([NotNull] MGPLParser.VarlistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.varlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarlist([NotNull] MGPLParser.VarlistContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.namelist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNamelist([NotNull] MGPLParser.NamelistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.namelist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNamelist([NotNull] MGPLParser.NamelistContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.explist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExplist([NotNull] MGPLParser.ExplistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.explist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExplist([NotNull] MGPLParser.ExplistContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExp([NotNull] MGPLParser.ExpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExp([NotNull] MGPLParser.ExpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.prefixexp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrefixexp([NotNull] MGPLParser.PrefixexpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.prefixexp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrefixexp([NotNull] MGPLParser.PrefixexpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.functioncall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctioncall([NotNull] MGPLParser.FunctioncallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.functioncall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctioncall([NotNull] MGPLParser.FunctioncallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.varOrExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarOrExp([NotNull] MGPLParser.VarOrExpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.varOrExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarOrExp([NotNull] MGPLParser.VarOrExpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.var"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVar([NotNull] MGPLParser.VarContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.var"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVar([NotNull] MGPLParser.VarContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.varSuffix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarSuffix([NotNull] MGPLParser.VarSuffixContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.varSuffix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarSuffix([NotNull] MGPLParser.VarSuffixContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.nameAndArgs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNameAndArgs([NotNull] MGPLParser.NameAndArgsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.nameAndArgs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNameAndArgs([NotNull] MGPLParser.NameAndArgsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.args"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgs([NotNull] MGPLParser.ArgsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.args"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgs([NotNull] MGPLParser.ArgsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.functiondef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctiondef([NotNull] MGPLParser.FunctiondefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.functiondef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctiondef([NotNull] MGPLParser.FunctiondefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.funcbody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFuncbody([NotNull] MGPLParser.FuncbodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.funcbody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFuncbody([NotNull] MGPLParser.FuncbodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.parlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParlist([NotNull] MGPLParser.ParlistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.parlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParlist([NotNull] MGPLParser.ParlistContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.tableconstructor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTableconstructor([NotNull] MGPLParser.TableconstructorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.tableconstructor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTableconstructor([NotNull] MGPLParser.TableconstructorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.fieldlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldlist([NotNull] MGPLParser.FieldlistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.fieldlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldlist([NotNull] MGPLParser.FieldlistContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterField([NotNull] MGPLParser.FieldContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitField([NotNull] MGPLParser.FieldContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.fieldsep"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldsep([NotNull] MGPLParser.FieldsepContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.fieldsep"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldsep([NotNull] MGPLParser.FieldsepContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.operatorOr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperatorOr([NotNull] MGPLParser.OperatorOrContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.operatorOr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperatorOr([NotNull] MGPLParser.OperatorOrContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.operatorAnd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperatorAnd([NotNull] MGPLParser.OperatorAndContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.operatorAnd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperatorAnd([NotNull] MGPLParser.OperatorAndContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.operatorComparison"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperatorComparison([NotNull] MGPLParser.OperatorComparisonContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.operatorComparison"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperatorComparison([NotNull] MGPLParser.OperatorComparisonContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.operatorStrcat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperatorStrcat([NotNull] MGPLParser.OperatorStrcatContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.operatorStrcat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperatorStrcat([NotNull] MGPLParser.OperatorStrcatContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.operatorAddSub"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperatorAddSub([NotNull] MGPLParser.OperatorAddSubContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.operatorAddSub"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperatorAddSub([NotNull] MGPLParser.OperatorAddSubContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.operatorMulDivMod"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperatorMulDivMod([NotNull] MGPLParser.OperatorMulDivModContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.operatorMulDivMod"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperatorMulDivMod([NotNull] MGPLParser.OperatorMulDivModContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.operatorBitwise"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperatorBitwise([NotNull] MGPLParser.OperatorBitwiseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.operatorBitwise"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperatorBitwise([NotNull] MGPLParser.OperatorBitwiseContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.operatorUnary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperatorUnary([NotNull] MGPLParser.OperatorUnaryContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.operatorUnary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperatorUnary([NotNull] MGPLParser.OperatorUnaryContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.operatorPower"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperatorPower([NotNull] MGPLParser.OperatorPowerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.operatorPower"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperatorPower([NotNull] MGPLParser.OperatorPowerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNumber([NotNull] MGPLParser.NumberContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNumber([NotNull] MGPLParser.NumberContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MGPLParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterString([NotNull] MGPLParser.StringContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MGPLParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitString([NotNull] MGPLParser.StringContext context);
}