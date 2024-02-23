using SourceCodeGeneration.Application.Command;
using System.Text;

namespace SourceCodeGeneration.Application;

/// <summary>
/// 値オブジェクト生成サービス
/// </summary>
public class ValueObjectGenerationService : IValueObjectGenerationService
{
	#region Methods

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	public Stream GenerateValueObject(ValueObjectMaximumLengthGenerationCommand command)
	{
		var stream = new MemoryStream();

		var writer = new StreamWriter(stream, Encoding.UTF8);

		// Namespace
		writer.Write("namespace ");
		writer.Write(command.NamespaceSequence);
		writer.WriteLine(";");

		writer.WriteLine();

		// Class - Documentation comment
		writer.WriteLine("/// <summary>");
		writer.Write("/// ");
		writer.WriteLine(command.ClassDescription);
		writer.WriteLine("/// </summary>");

		// Class - Begin: Definition
		writer.Write("public record class ");
		writer.WriteLine(command.ClassName);
		writer.WriteLine("{");

		// Constructor
		{
			// Begin: Region derective
			writer.WriteLine("    #region Constructors");
			writer.WriteLine();

			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.Write("    /// ");
			writer.Write(command.ClassDescription);
			writer.WriteLine("を初期化します。");
			writer.WriteLine("    /// </summary>");
			writer.WriteLine("    /// <param name=\"value\">値</param>");

			// Begin: Definition
			writer.Write("    public ");
			writer.Write(command.ClassName);
			writer.WriteLine("(string value)");
			writer.WriteLine("    {");

			// Content
			writer.WriteLine("        bool succeeded = Validate(value, out string message);");
			writer.WriteLine("        if (!succeeded) throw new ArgumentException(message, nameof(value));");

			writer.WriteLine();

			writer.WriteLine("        Value = value;");

			// End: Definition
			writer.WriteLine("    }");

			// End: Region derective
			writer.WriteLine();
			writer.WriteLine("    #endregion");
		}

		writer.WriteLine();

		// Property
		{
			// Begin: Region derective
			writer.WriteLine("    #region Properties");
			writer.WriteLine();

			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.WriteLine("    /// 値を取得します。");
			writer.WriteLine("    /// </summary>");

			// Definition
			writer.WriteLine("    public string Value { get; }");

			// End: Region derective
			writer.WriteLine();
			writer.WriteLine("    #endregion");
		}

		writer.WriteLine();

		// Method
		{
			// Begin: Region derective
			writer.WriteLine("    #region Methods");
			writer.WriteLine();

			// Validate method
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.WriteLine("    /// 値を検証します。");
				writer.WriteLine("    /// </summary>");
				writer.WriteLine("    /// <param name=\"value\">値</param>");
				writer.WriteLine("    /// <param name=\"message\">メッセージ</param>");

				// Begin: Definition
				writer.WriteLine("    public static bool Validate(string value, out string message)");
				writer.WriteLine("    {");

				// Contents
				writer.Write("        const int maximumLength = ");
				writer.Write(command.MaximumLength);
				writer.WriteLine(";");

				writer.WriteLine("        bool result = value.Length <= maximumLength;");

				writer.WriteLine();

				writer.WriteLine("        if (result)");
				writer.WriteLine("        {");
				writer.WriteLine("            message = string.Empty;");
				writer.WriteLine("        }");
				writer.WriteLine("        else");
				writer.WriteLine("        {");
				writer.Write("            message = $\"");
				writer.Write(command.ClassDescription);
				writer.WriteLine("は、{maximumLength}桁以内で入力してください。\";");
				writer.WriteLine("        }");

				writer.WriteLine();

				writer.WriteLine("        return result;");

				// End: Definition
				writer.WriteLine("    }");
			}

			// End: Region derective
			writer.WriteLine();
			writer.WriteLine("    #endregion");
		}

		// Class - End: Definition
		writer.WriteLine("}");

		writer.Flush();

		return stream;
	}

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	public async Task<Stream> GenerateValueObjectAsync(ValueObjectMaximumLengthGenerationCommand command)
	{
		return await Task.Run(() => GenerateValueObject(command));
	}

	#endregion
}
