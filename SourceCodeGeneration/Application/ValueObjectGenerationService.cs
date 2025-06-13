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
	public Stream GenerateValueObject(ValueObjectNumberGenerationCommand command)
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

		if (!command.HasMinimumNumber && !command.HasMaximumNumber)
		{
			writer.WriteLine("/// <param name=\"Value\">値</param>");

			// Class - Begin: Definition
			writer.Write("public record class ");
			writer.Write(command.ClassName);
			writer.WriteLine("(int Value);");
		}
		else
		{
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
				writer.WriteLine("(int value)");
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
				writer.WriteLine("    public int Value { get; }");

				// End: Region derective
				writer.WriteLine();
				writer.WriteLine("    #endregion");
			}

			writer.WriteLine();

			// Operator
			if (command.HasPlusOperator || command.HasMinusOperator)
			{
				// Begin: Region derective
				writer.WriteLine("    #region Operators");
				writer.WriteLine();

				// + operator
				if (command.HasPlusOperator)
				{
					// Documentation comment
					writer.WriteLine("    /// <summary>");
					writer.WriteLine("    /// 左側のオペランドに右側のオペランドを加算します。");
					writer.WriteLine("    /// </summary>");
					writer.WriteLine("    /// <param name=\"lhs\">左側のオペランド</param>");
					writer.WriteLine("    /// <param name=\"rhs\">右側のオペランド</param>");
					writer.Write("    /// <returns>加算した");
					writer.Write(command.ClassDescription);
					writer.WriteLine("を返します。</returns>");

					// Begin: Definition
					writer.Write("    public static ");
					writer.Write(command.ClassName);
					writer.Write(" operator +(");
					writer.Write(command.ClassName);
					writer.Write(" lhs, ");
					writer.Write(command.ClassName);
					writer.WriteLine(" rhs)");
					writer.WriteLine("    {");

					// Contents
					writer.Write("        ");
					writer.Write(command.ClassName);
					writer.WriteLine(" result = new(lhs.Value + rhs.Value);");
					writer.WriteLine();
					writer.WriteLine("        return result;");

					// End: Definition
					writer.WriteLine("    }");

					writer.WriteLine();
				}

				// - operator
				if (command.HasMinusOperator)
				{
					// Documentation comment
					writer.WriteLine("    /// <summary>");
					writer.WriteLine("    /// 左側のオペランドから右側のオペランドを減算します。");
					writer.WriteLine("    /// </summary>");
					writer.WriteLine("    /// <param name=\"lhs\">左側のオペランド</param>");
					writer.WriteLine("    /// <param name=\"rhs\">右側のオペランド</param>");
					writer.Write("    /// <returns>減算した");
					writer.Write(command.ClassDescription);
					writer.WriteLine("を返します。</returns>");

					// Begin: Definition
					writer.Write("    public static ");
					writer.Write(command.ClassName);
					writer.Write(" operator -(");
					writer.Write(command.ClassName);
					writer.Write(" lhs, ");
					writer.Write(command.ClassName);
					writer.WriteLine(" rhs)");
					writer.WriteLine("    {");

					// Contents
					writer.Write("        ");
					writer.Write(command.ClassName);
					writer.WriteLine(" result = new(lhs.Value - rhs.Value);");
					writer.WriteLine();
					writer.WriteLine("        return result;");

					// End: Definition
					writer.WriteLine("    }");

					writer.WriteLine();
				}

				writer.WriteLine("    #endregion");

				writer.WriteLine();
			}

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
					writer.WriteLine("    public static bool Validate(int value, out string message)");
					writer.WriteLine("    {");

					// Contents
					if (command.HasMinimumNumber)
					{
						writer.Write("        const int minimumValue = ");
						writer.Write(command.MinimumNumber.ToString("#,0").Replace(",", "_"));
						writer.WriteLine(";");
					}

					if (command.HasMaximumNumber)
					{
						writer.Write("        const int maximumValue = ");
						writer.Write(command.MaximumNumber.ToString("#,0").Replace(",", "_"));
						writer.WriteLine(";");
					}

					writer.Write("        bool result = ");

					if (command.HasMinimumNumber)
					{
						writer.Write("minimumValue <= value");
					}

					if (command.HasMinimumNumber && command.HasMaximumNumber)
					{
						writer.Write(" && ");
					}

					if (command.HasMaximumNumber)
					{
						writer.Write("value <= maximumValue");
					}

					writer.WriteLine(";");

					writer.WriteLine();

					writer.WriteLine("        if (result)");
					writer.WriteLine("        {");
					writer.WriteLine("            message = string.Empty;");
					writer.WriteLine("        }");
					writer.WriteLine("        else");
					writer.WriteLine("        {");
					writer.Write("            message = $\"");
					writer.Write(command.ClassDescription);
					writer.Write("は、");

					if (command.HasMinimumNumber && command.HasMaximumNumber)
					{
						writer.Write("{minimumValue:#,0} ～ {maximumValue:#,0} の間で入力してください。");
					}
					else if (command.HasMinimumNumber)
					{
						writer.Write("{minimumValue:#,0} 以上で入力してください。");
					}
					else if (command.HasMaximumNumber)
					{
						writer.Write("{maximumValue:#,0} 以下で入力してください。");
					}

					writer.WriteLine("\";");

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
		}

		writer.Flush();

		return stream;
	}

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	public Stream GenerateValueObject(ValueObjectStringPatternGenerationCommand command)
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
				writer.Write("は、");
				writer.Write(command.PatternDescription);
				writer.WriteLine("で入力してください。\";");
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
				writer.Write("    [GeneratedRegex(@\"");
				writer.Write(command.Pattern);
				writer.WriteLine("\")]");

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
	public Stream GenerateValueObject(ValueObjectUuidGenerationCommand command)
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
	public async Task<Stream> GenerateValueObjectAsync(ValueObjectMaximumLengthGenerationCommand command)
	{
		return await Task.Run(() => GenerateValueObject(command));
	}

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	public async Task<Stream> GenerateValueObjectAsync(ValueObjectNumberGenerationCommand command)
	{
		return await Task.Run(() => GenerateValueObject(command));
	}

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	public async Task<Stream> GenerateValueObjectAsync(ValueObjectStringPatternGenerationCommand command)
	{
		return await Task.Run(() => GenerateValueObject(command));
	}

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	public async Task<Stream> GenerateValueObjectAsync(ValueObjectUuidGenerationCommand command)
	{
		return await Task.Run(() => GenerateValueObject(command));
	}

	#endregion
}
