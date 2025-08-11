using SourceCodeGeneration.Application.Command;
using System.Text;

namespace SourceCodeGeneration.Application;

/// <summary>
/// エンティティ生成サービス
/// </summary>
public class EntityGenerationService : IEntityGenerationService
{
	#region Methods

	/// <summary>
	/// エンティティを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	public Stream GenerateEntity(EntityGenerationCommand command)
	{
		var properties = command.Properties.Select(property => new PropertyTray(property));
		var firstProperty = properties.First();
		var secondAndSubsequentProperties = properties.Skip(1);

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

		foreach (var property in properties)
		{
			writer.Write("/// <param name=\"");
			writer.Write(property.ParameterName);
			writer.Write("\">");
			writer.Write(property.PropertyDescription);
			writer.WriteLine("</param>");
		}

		// Class - Begin: Definition
		writer.Write("public class ");
		writer.Write(command.ClassName);
		writer.Write("(");

		static void writeParameter(StreamWriter writer, PropertyTray property)
		{
			writer.Write(property.TypeName);
			writer.Write(" ");
			writer.Write(property.ParameterName);
		}

		writeParameter(writer, firstProperty);
		foreach (var property in secondAndSubsequentProperties)
		{
			writer.Write(", ");
			writeParameter(writer, property);
		}

		writer.Write(") : IEquatable<");
		writer.Write(command.ClassName);
		writer.WriteLine(">");
		writer.WriteLine("{");

		// Property
		{
			// Begin: Region derective
			writer.WriteLine("    #region Properties");
			writer.WriteLine();

			static void writeProperty(StreamWriter writer, PropertyTray property)
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.Write("    /// ");
				writer.Write(property.PropertyDescription);
				writer.WriteLine("を取得します。");
				writer.WriteLine("    /// </summary>");

				// Begin: Definition
				writer.Write("    public ");
				writer.Write(property.TypeName);
				writer.Write(" ");
				writer.Write(property.PropertyName);
				writer.Write(" { get; } = ");
				writer.Write(property.ParameterName);
				writer.WriteLine(";");
			}

			writeProperty(writer, firstProperty);
			foreach (var property in secondAndSubsequentProperties)
			{
				writer.WriteLine();
				writeProperty(writer, property);
			}

			// End: Region derective
			writer.WriteLine();
			writer.WriteLine("    #endregion");
		}

		writer.WriteLine();

		// Operator
		{
			// Begin: Region derective
			writer.WriteLine("    #region Operators");
			writer.WriteLine();

			// == operator
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.WriteLine("    /// オペランドが等しい場合には <c>true</c> を返し、それ以外の場合は <c>false</c> を返します。");
				writer.WriteLine("    /// </summary>");
				writer.WriteLine("    /// <param name=\"lhs\">左辺</param>");
				writer.WriteLine("    /// <param name=\"rhs\">右辺</param>");
				writer.WriteLine("    /// <returns>オペランドが等しい場合は、 <c>true</c>。それ以外の場合は、 <c>false</c>。</returns>");

				// Begin: Definition
				writer.Write("    public static bool operator ==(");
				writer.Write(command.ClassName);
				writer.Write(" lhs, ");
				writer.Write(command.ClassName);
				writer.WriteLine(" rhs)");
				writer.WriteLine("    {");

				// Content
				writer.WriteLine("        if (lhs is null) return rhs is null;");
				writer.WriteLine();
				writer.WriteLine("        bool result = lhs.Equals(rhs);");
				writer.WriteLine();
				writer.WriteLine("        return result;");

				// End: Definition
				writer.WriteLine("    }");
			}

			writer.WriteLine();

			// != operator
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.WriteLine("    /// オペランドが等しくない場合には <c>true</c> を返し、それ以外の場合は <c>false</c> を返します。");
				writer.WriteLine("    /// </summary>");
				writer.WriteLine("    /// <param name=\"lhs\">左辺</param>");
				writer.WriteLine("    /// <param name=\"rhs\">右辺</param>");
				writer.WriteLine("    /// <returns>オペランドが等しくない場合は、 <c>true</c>。それ以外の場合は、 <c>false</c>。</returns>");

				// Begin: Definition
				writer.Write("    public static bool operator !=(");
				writer.Write(command.ClassName);
				writer.Write(" lhs, ");
				writer.Write(command.ClassName);
				writer.WriteLine(" rhs)");
				writer.WriteLine("    {");

				// Content
				writer.WriteLine("        bool result = !(lhs == rhs);");
				writer.WriteLine();
				writer.WriteLine("        return result;");

				// End: Definition
				writer.WriteLine("    }");
			}

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

			// object.Equals method
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.WriteLine("    /// 指定されたオブジェクトが現在のオブジェクトと等しいかどうかを判断します。");
				writer.WriteLine("    /// </summary>");
				writer.WriteLine("    /// <param name=\"obj\">現在のオブジェクトと比較するオブジェクト。</param>");
				writer.WriteLine("    /// <returns>指定したオブジェクトが現在のオブジェクトと等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>");

				// Begin: Definition
				writer.WriteLine("    public override bool Equals(object? obj)");
				writer.WriteLine("    {");

				// Content
				writer.WriteLine("        bool result = obj switch");
				writer.WriteLine("        {");
				writer.Write("            ");
				writer.Write(command.ClassName);
				writer.WriteLine(" other => Equals(other),");
				writer.WriteLine("            _ => base.Equals(obj),");
				writer.WriteLine("        };");
				writer.WriteLine();
				writer.WriteLine("        return result;");

				// End: Definition
				writer.WriteLine("    }");
			}

			writer.WriteLine();

			// IEquatable<T>.Equals method
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.WriteLine("    /// 現在のオブジェクトが、同じ型の別のオブジェクトと等しいかどうかを示します。");
				writer.WriteLine("    /// </summary>");
				writer.WriteLine("    /// <param name=\"other\">このオブジェクトと比較するオブジェクト。</param>");
				writer.WriteLine("    /// <returns>現在のオブジェクトが <c>other</c> パラメーターと等しい場合は <c>true</c>、それ以外の場合は <c>false</c> です。</returns>");

				// Begin: Definition
				writer.Write("    public bool Equals(");
				writer.Write(command.ClassName);
				writer.WriteLine("? other)");
				writer.WriteLine("    {");

				// Content
				writer.WriteLine("        if (other is null) return false;");
				writer.WriteLine();

				{
					writer.Write("        bool result = ");

					PropertyTray firstPrimaryKey;
					IEnumerable<PropertyTray> secondAfterPrimaryKeys;
					{
						IEnumerable<PropertyTray> primaryKeys = properties.Where(x => x.IsPrimaryKey);

						firstPrimaryKey = primaryKeys.First();
						secondAfterPrimaryKeys = primaryKeys.Skip(1);
					}

					static void writePrimaryKey(StreamWriter writer, PropertyTray property)
					{
						writer.Write(property.PropertyName);
						writer.Write(" == other.");
						writer.Write(property.PropertyName);
					}

					writePrimaryKey(writer, firstPrimaryKey);
					foreach (PropertyTray primaryKey in secondAfterPrimaryKeys)
					{
						writer.Write(" && ");
						writePrimaryKey(writer, primaryKey);
					}

					writer.WriteLine(";");
				}

				writer.WriteLine();
				writer.WriteLine("        return result;");

				// End: Definition
				writer.WriteLine("    }");
			}

			writer.WriteLine();

			// GetHashCode method
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.WriteLine("    /// 既定のハッシュ関数として機能します。");
				writer.WriteLine("    /// </summary>");
				writer.WriteLine("    /// <returns>現在のオブジェクトのハッシュ コード。</returns>");

				// Begin: Definition
				writer.WriteLine("    public override int GetHashCode()");
				writer.WriteLine("    {");

				// Content
				writer.Write("        int result = HashCode.Combine(");
				writer.Write(firstProperty.PropertyName);
				writer.WriteLine(");");
				writer.WriteLine();
				writer.WriteLine("        return result;");

				// End: Definition
				writer.WriteLine("    }");
			}

			writer.WriteLine();

			// ToString Method
			{
				// Documentation comment
				writer.WriteLine("    /// <summary>");
				writer.WriteLine("    /// 現在のオブジェクトを表す文字列を返します。");
				writer.WriteLine("    /// </summary>");
				writer.WriteLine("    /// <returns>現在のオブジェクトを表す文字列。</returns>");

				// Begin: Definition
				writer.WriteLine("    public override string ToString()");
				writer.WriteLine("    {");
				writer.Write("        string str = $\"{nameof(");
				writer.Write(command.ClassName);
				writer.Write(")} {{ ");

				static void writeProperty(StreamWriter writer, PropertyTray property)
				{
					writer.Write("{nameof(");
					writer.Write(property.PropertyName);
					writer.Write(")} = {");
					writer.Write(property.PropertyName);
					writer.Write("}");
				}

				writeProperty(writer, firstProperty);
				foreach (var property in secondAndSubsequentProperties)
				{
					writer.Write(", ");
					writeProperty(writer, property);
				}

				// End: Definition
				writer.WriteLine(" }}\";");
				writer.WriteLine();
				writer.WriteLine("        return str;");
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
	/// エンティティを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	public async Task<Stream> GenerateEntityAsync(EntityGenerationCommand command)
	{
		return await Task.Run(() => GenerateEntity(command));
	}

	#endregion
}
