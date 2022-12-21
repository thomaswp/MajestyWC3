//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:\Users\Thomas\source\repos\WCSharpTemplate\MapImporter\MGPL.g4 by ANTLR 4.9.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="MGPLParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.2")]
[System.CLSCompliant(false)]
public interface IMGPLVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.chunk"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitChunk([NotNull] MGPLParser.ChunkContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] MGPLParser.BlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStat([NotNull] MGPLParser.StatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment([NotNull] MGPLParser.AssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.if_stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIf_stat([NotNull] MGPLParser.If_statContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.stat_or_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStat_or_block([NotNull] MGPLParser.Stat_or_blockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.func_dec"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunc_dec([NotNull] MGPLParser.Func_decContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.func_variables"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunc_variables([NotNull] MGPLParser.Func_variablesContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.func_var_dec"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunc_var_dec([NotNull] MGPLParser.Func_var_decContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.func_body"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunc_body([NotNull] MGPLParser.Func_bodyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] MGPLParser.TypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.attnamelist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAttnamelist([NotNull] MGPLParser.AttnamelistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.attrib"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAttrib([NotNull] MGPLParser.AttribContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.laststat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLaststat([NotNull] MGPLParser.LaststatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLabel([NotNull] MGPLParser.LabelContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.funcname"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncname([NotNull] MGPLParser.FuncnameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.varlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarlist([NotNull] MGPLParser.VarlistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.namelist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNamelist([NotNull] MGPLParser.NamelistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.explist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExplist([NotNull] MGPLParser.ExplistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.exp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExp([NotNull] MGPLParser.ExpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.prefixexp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrefixexp([NotNull] MGPLParser.PrefixexpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.functioncall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctioncall([NotNull] MGPLParser.FunctioncallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.varOrExp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarOrExp([NotNull] MGPLParser.VarOrExpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.var"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVar([NotNull] MGPLParser.VarContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.varSuffix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarSuffix([NotNull] MGPLParser.VarSuffixContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.nameAndArgs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNameAndArgs([NotNull] MGPLParser.NameAndArgsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.args"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgs([NotNull] MGPLParser.ArgsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.functiondef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctiondef([NotNull] MGPLParser.FunctiondefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.funcbody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncbody([NotNull] MGPLParser.FuncbodyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.parlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParlist([NotNull] MGPLParser.ParlistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.tableconstructor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTableconstructor([NotNull] MGPLParser.TableconstructorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.fieldlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFieldlist([NotNull] MGPLParser.FieldlistContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitField([NotNull] MGPLParser.FieldContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.fieldsep"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFieldsep([NotNull] MGPLParser.FieldsepContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.operatorOr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorOr([NotNull] MGPLParser.OperatorOrContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.operatorAnd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorAnd([NotNull] MGPLParser.OperatorAndContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.operatorComparison"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorComparison([NotNull] MGPLParser.OperatorComparisonContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.operatorStrcat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorStrcat([NotNull] MGPLParser.OperatorStrcatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.operatorAddSub"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorAddSub([NotNull] MGPLParser.OperatorAddSubContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.operatorMulDivMod"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorMulDivMod([NotNull] MGPLParser.OperatorMulDivModContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.operatorBitwise"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorBitwise([NotNull] MGPLParser.OperatorBitwiseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.operatorUnary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorUnary([NotNull] MGPLParser.OperatorUnaryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.operatorPower"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperatorPower([NotNull] MGPLParser.OperatorPowerContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber([NotNull] MGPLParser.NumberContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MGPLParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] MGPLParser.StringContext context);
}
