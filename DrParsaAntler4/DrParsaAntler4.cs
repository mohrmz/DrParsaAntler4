using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

using System;
using System.IO;
using System.Windows.Forms;

namespace DrParsaAntler4
{
    public partial class DrParsaAntler4 : Form
    {
        public DrParsaAntler4()
        {
            InitializeComponent();
        }
        public static string Parser(string input)
        {
            var result="";
            try
            {
                AntlrInputStream antlrInputStream = new AntlrInputStream(input);
                ExprLexer Lexer = new ExprLexer(antlrInputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(Lexer);
                ExprParser parser = new ExprParser(commonTokenStream);
                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ThrowExceptionErrorListener());
                var ParseTree = parser.prog();
                exprvisit visitor = new exprvisit();
                result = visitor.Visit(ParseTree).ToString();
                return result;
            }
           catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }
       
        public  class exprvisit : ExprBaseVisitor<double>
        {
            public override double VisitNumber([NotNull] ExprParser.NumberContext context)
            {
                int size = context.ChildCount;
                if (size == 2)
                {
                    
                        return double.Parse((context.GetChild(1).GetText()));               
                }
                else
                {
                    return double.Parse(context.GetChild(0).GetText());
                }
            }
            public override double VisitAddSub([NotNull] ExprParser.AddSubContext context)
            {
                double left = Visit(context.expr(0));
                double right = Visit(context.expr(1));
                if (context.op.Type == ExprParser.ADD)
                    return left + right;
                return left - right;
            }
            public override double VisitParens([NotNull] ExprParser.ParensContext context)
            {
                return Visit(context.expr());
            }
            public override double VisitId([NotNull] ExprParser.IdContext context)
            {
                throw new Exception("Detect ID In Right Hand Side ! Result Can not Calculate");
            }
            public override double VisitMulDiv([NotNull] ExprParser.MulDivContext context)
            {
                double left = Visit(context.expr(0));
                double right = Visit(context.expr(1));
                if (context.op.Type == ExprParser.MUL)
                {
                    return left * right;
                }
                else
                {
                    if (right == 0)
                    {
                        throw new DivideByZeroException("You can't divide by Zero!");
                    }
                    else
                    {
                        return left / right;
                    }
                }
                
            }
        }
        public class ThrowExceptionErrorListener : BaseErrorListener
        {
            public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
            {
                throw new ArgumentException("Invalid Expression: {0}", msg, e);
            }
           
        }

        private void Parse_Click(object sender, EventArgs e)
        {

            lblresult.Text = Parser(textBox1.Text);
        }
    }
}
