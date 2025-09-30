using SourceCodeGeneration.Application.ValueObject.Command;
using System.Text;

namespace SourceCodeGeneration.Application.ValueObject;

/// <summary>
/// 値オブジェクト(UUID)の生成サービス
/// </summary>
public class UuidGenerationService : IUuidGenerationService
{
	#region Methods

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	public Stream Generate(UuidGenerationCommand command)
	{
		var stream = new MemoryStream();

		var writer = new StreamWriter(stream, Encoding.UTF8);

		// Using directive
		writer.WriteLine("using System.Text.RegularExpressions;");

		writer.WriteLine();

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
		writer.Write("public partial record class ");
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

			// Create method
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.Write("    /// ");
				writer.Write(command.ClassDescription);
				writer.WriteLine("を作成します。");
				writer.WriteLine("    /// </summary>");
				writer.Write("    /// <returns>作成した");
				writer.Write(command.ClassDescription);
				writer.WriteLine("を返します。</returns>");

				// Begin: Definition
				writer.Write("    public static ");
				writer.Write(command.ClassName);
				writer.WriteLine(" Create()");
				writer.WriteLine("    {");

				// Contents
				writer.Write("        ");
				writer.Write(command.ClassName);
				writer.WriteLine(" product;");
				writer.WriteLine("        {");
				writer.WriteLine("            string value = Guid.NewGuid().ToString().ToLower();");
				writer.WriteLine();
				writer.Write("            product = new ");
				writer.Write(command.ClassName);
				writer.WriteLine("(value);");
				writer.WriteLine("        }");

				writer.WriteLine();

				writer.WriteLine("        return product;");

				// End: Definition
				writer.WriteLine("    }");
			}

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
				writer.WriteLine("        bool result = GetRegex().IsMatch(value);");

				writer.WriteLine();

				writer.WriteLine("        if (result)");
				writer.WriteLine("        {");
				writer.WriteLine("            message = string.Empty;");
				writer.WriteLine("        }");
				writer.WriteLine("        else");
				writer.WriteLine("        {");
				writer.Write("            message = \"");
				writer.Write(command.ClassDescription);
				writer.WriteLine("は、UUID の形式で入力してください。\";");
				writer.WriteLine("        }");

				writer.WriteLine();

				writer.WriteLine("        return result;");

				// End: Definition
				writer.WriteLine("    }");
			}

			writer.WriteLine();

			// GetRegex method
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.WriteLine("    /// 正規表現を取得します。");
				writer.WriteLine("    /// </summary>");
				writer.WriteLine("    /// <returns>正規表現を返します。</returns>");

				// Argument
				writer.WriteLine("    [GeneratedRegex(@\"^[0-9a-f]{8}\\-[0-9a-f]{4}\\-[0-9a-f]{4}\\-[0-9a-f]{4}\\-[0-9a-f]{12}$\")]");

				// Begin: Definition
				writer.WriteLine("    private static partial Regex GetRegex();");
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
	public async Task<Stream> GenerateAsync(UuidGenerationCommand command)
	{
		return await Task.Run(() => Generate(command));
	}

	#endregion
}
