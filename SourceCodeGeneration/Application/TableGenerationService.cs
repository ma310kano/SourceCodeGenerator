using SourceCodeGeneration.Application.Command;
using SourceCodeGeneration.Data;
using System.Text;

namespace SourceCodeGeneration.Application;

/// <summary>
/// テーブルの生成サービス
/// </summary>
public class TableGenerationService : ITableGenerationService
{
	#region Methods

	/// <summary>
	/// CREATE TABLE 文を生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public GenerationResult GenerateCreateTableStatement(TableGenerationCommand command)
	{
		TableColumnGenerationCommand firstColumn = command.Columns.First();
		TableColumnGenerationCommand[] secondAfterColumns = command.Columns.Skip(1).ToArray();

		TableColumnGenerationCommand[] keyColumns = command.Columns.Where(x => x.IsPrimaryKey).ToArray();
		TableColumnGenerationCommand firstKeyColumn = keyColumns.First();
		TableColumnGenerationCommand[] secondAfterKeyColumns = keyColumns.Skip(1).ToArray();

		MemoryStream stream = new();

		StreamWriter writer = new(stream, Encoding.UTF8);

		writer.WriteLine("CREATE TABLE ");
		writer.Write("    ");
		writer.Write(command.PluralName);
		writer.WriteLine(" ");
		writer.WriteLine("    ( ");

		void WriteColumn(string separator, TableColumnGenerationCommand column)
		{
			writer.Write("        ");
			writer.Write(separator);
			writer.Write(column.ColumnName);
			writer.Write(" ");

			switch (column.ColumnType)
			{
				case ColumnTypes.String:
					writer.Write("TEXT");
					break;
				case ColumnTypes.Integer:
					writer.Write("INTEGER");
					break;
			}

			if (column.IsNotNull)
			{
				writer.Write(" NOT NULL");
			}

			writer.WriteLine(" ");
		}

		WriteColumn("  ", firstColumn);
		foreach (TableColumnGenerationCommand column in secondAfterColumns)
		{
			WriteColumn(", ", column);
		}

		writer.Write("        , PRIMARY KEY ( ");

		writer.Write(firstKeyColumn.ColumnName);
		foreach (TableColumnGenerationCommand column in secondAfterKeyColumns)
		{
			writer.Write(", ");
			writer.Write(firstColumn.ColumnName);
		}
		
		writer.WriteLine(" ) ");

		writer.WriteLine("    );");

		writer.Flush();
		stream.Position = 0;

		GenerationResult result;
		{
			string fileName = command.PluralName + ".sql";

			result = new GenerationResult(fileName, stream);
		}

		return result;
	}

	/// <summary>
	/// CREATE TABLE 文を生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public async Task<GenerationResult> GenerateCreateTableStatementAsync(TableGenerationCommand command)
	{
		return await Task.Run(() => GenerateCreateTableStatement(command));
	}

	/// <summary>
	/// データクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public GenerationResult GenerateDataClass(TableGenerationCommand command)
	{
		string className;
		{
			TableName tableName = new(command.SingularName);
			className = tableName.PascalCase + "Data";
		}

		Column[] columns = command.Columns.Select(x => new Column(new ColumnName(x.ColumnName), x.ColumnType, x.Description, x.IsNotNull, x.IsPrimaryKey, x.ContainsSummary)).ToArray();
		Column firstColumn = columns.First();
		Column[] secondAfterColumns = columns.Skip(1).ToArray();

		MemoryStream stream = new();

		StreamWriter writer = new(stream, Encoding.UTF8);

		// Namespace
		writer.Write("namespace ");
		writer.Write(command.NamespaceSequence);
		writer.WriteLine(".Application.Data;");

		writer.WriteLine();

		// Class - Documentation comment
		writer.WriteLine("/// <summary>");
		writer.Write("/// ");
		writer.Write(command.Description);
		writer.WriteLine("データ");
		writer.WriteLine("/// </summary>");

		foreach (Column column in columns)
		{
			writer.Write("/// <param name=\"");
			writer.Write(column.ColumnName.PascalCase);
			writer.Write("\">");
			writer.Write(column.Description);
			writer.WriteLine("</param>");
		}

		// Class - Begin: Definition
		writer.Write("public record class ");
		writer.Write(className);
		writer.Write("(");

		void writeProperty(Column column)
		{
			switch (column.Type)
			{
				case ColumnTypes.String:
					writer.Write("string");
					break;
				case ColumnTypes.Integer:
					writer.Write("int");
					break;
			}

			if (!column.IsNotNull)
			{
				writer.Write("?");
			}

			writer.Write(" ");
			writer.Write(column.ColumnName.PascalCase);
		}

		writeProperty(firstColumn);
		foreach (Column column in secondAfterColumns)
		{
			writer.Write(", ");
			writeProperty(column);
		}

		writer.WriteLine(");");

		writer.Flush();
		stream.Position = 0;

		GenerationResult result;
		{
			string fileName = className + ".cs";

			result = new GenerationResult(fileName, stream);
		}

		return result;
	}

	/// <summary>
	/// データクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public async Task<GenerationResult> GenerateDataClassAsync(TableGenerationCommand command)
	{
		return await Task.Run(() => GenerateDataClass(command));
	}

	/// <summary>
	/// クエリサービスのクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public GenerationResult GenerateQueryServiceClass(TableGenerationCommand command)
	{
		string namespaceSequence = command.NamespaceSequence + ".Application";

		string className;
		string interfaceName;
		string dataClassName;
		{
			TableName tableName = new(command.SingularName);

			className = tableName.PascalCase + "QueryService";
			interfaceName = "I" + className;
			dataClassName = tableName.PascalCase + "Data";
		}

		Column[] columns = command.Columns.Select(x => new Column(new ColumnName(x.ColumnName), x.ColumnType, x.Description, x.IsNotNull, x.IsPrimaryKey, x.ContainsSummary)).ToArray();
		Column firstColumn = columns.First();
		Column[] secondAfterColumns = columns.Skip(1).ToArray();

		Column[] keyColumns = columns.Where(x => x.IsPrimaryKey).ToArray();
		Column firstKeyColumn = keyColumns.First();
		Column[] secondAfterKeyColumns = keyColumns.Skip(1).ToArray();

		MemoryStream stream = new();

		StreamWriter writer = new(stream, Encoding.UTF8);

		// Using
		{
			List<string> references = [
				"Dapper",
				"Microsoft.Extensions.Configuration",
				namespaceSequence + ".Data",
			];

			references.Sort();

			foreach (string reference in references)
			{
				writer.Write("using ");
				writer.Write(reference);
				writer.WriteLine(";");
			}
		}

		// Blank line: Using - Namespace
		writer.WriteLine();

		// Namespace
		writer.Write("namespace ");
		writer.Write(namespaceSequence);
		writer.WriteLine(".Sqlite;");

		// Blank line: Namespace - Interface
		writer.WriteLine();

		// Class: Documentation comment
		writer.WriteLine("/// <summary>");
		writer.Write("/// ");
		writer.Write(command.Description);
		writer.WriteLine("の問い合わせサービス");
		writer.WriteLine("/// </summary>");
		writer.WriteLine("/// <param name=\"configuration\">設定</param>");

		// Class: Definition
		writer.Write("public class ");
		writer.Write(className);
		writer.Write("(IConfiguration configuration) : ");
		writer.WriteLine(interfaceName);
		writer.WriteLine("{");

		// Fields: Begin
		writer.WriteLine("    #region Fields");
		writer.WriteLine();

		// Field: _connectionString
		writer.WriteLine("    /// <summary>");
		writer.WriteLine("    /// 接続文字列");
		writer.WriteLine("    /// </summary>");
		writer.Write("    private readonly string _connectionString = configuration.GetConnectionString(\"");
		writer.Write(command.ConnectionStringKey);
		writer.WriteLine("\") ?? throw new InvalidOperationException(\"接続文字列が取得できません。\");");

		// Fields: End
		writer.WriteLine();
		writer.WriteLine("    #endregion");

		// Blank line: Fields - Methods
		writer.WriteLine();

		// Methods: Begin
		writer.WriteLine("    #region Methods");
		writer.WriteLine();

		void WriteDocumentationCommentParameter(Column[] keyColumns)
		{
			foreach (Column keyColumn in keyColumns)
			{
				writer.Write("    /// <param name=\"");
				writer.Write(keyColumn.ColumnName.CamelCase);
				writer.Write("\">");
				writer.Write(keyColumn.Description);
				writer.WriteLine("</param>");
			}
		}

		void WriteMethodParameter(Column column)
		{
			switch (column.Type)
			{
				case ColumnTypes.String:
					writer.Write("string");
					break;
				case ColumnTypes.Integer:
					writer.Write("int");
					break;
			}

			writer.Write(" ");
			writer.Write(column.ColumnName.CamelCase);
		}

		// Methods: Query method
		{
			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.Write("    /// ");
			writer.Write(command.Description);
			writer.WriteLine("を問い合わせします。");
			writer.WriteLine("    /// </summary>");

			WriteDocumentationCommentParameter(keyColumns);

			writer.Write("    /// <returns>問い合わせした");
			writer.Write(command.Description);
			writer.WriteLine("データを返します。</returns>");

			// Definition
			writer.Write("    public ");
			writer.Write(dataClassName);
			writer.Write(" QuerySingle(");

			// Parameter
			WriteMethodParameter(firstKeyColumn);
			foreach (Column keyColumn in secondAfterKeyColumns)
			{
				WriteMethodParameter(keyColumn);
			}

			writer.WriteLine(")");

			writer.WriteLine("    {");

			writer.WriteLine("        using SqliteConnection connection = new(_connectionString);");
			writer.WriteLine("        connection.Open();");

			writer.WriteLine();

			writer.WriteLine("        const string sql = @\"SELECT");

			void WriteColumn(string separator, Column column)
			{
				writer.Write("    ");

				if (separator != string.Empty)
				{
					writer.Write(separator);
				}

				writer.WriteLine(column.ColumnName.SnakeCase);
			}

			if (columns.Length > 1)
			{
				WriteColumn("  ", firstColumn);

				foreach (Column column in secondAfterColumns)
				{
					WriteColumn(", ", column);
				}
			}
			else if (columns.Length == 1)
			{
				WriteColumn(string.Empty, firstColumn);
			}

			writer.WriteLine("FROM ");
			writer.Write("    ");
			writer.WriteLine(command.PluralName);
			writer.WriteLine("WHERE");

			void WriteWhereCondition(string separator, Column column)
			{
				writer.Write("    ");

				if (separator != string.Empty)
				{
					writer.Write(separator);
				}

				writer.Write(column.ColumnName.SnakeCase);
				writer.Write(" = :");
				writer.Write(column.ColumnName.SnakeCase);
			}

			if (keyColumns.Length == 1)
			{
				WriteWhereCondition(string.Empty, firstColumn);
			}
			else if (keyColumns.Length > 1)
			{
				Column[] middleKeyColumns = secondAfterKeyColumns.SkipLast(1).ToArray();
				Column lastKeyColumn = secondAfterKeyColumns.Last();

				WriteWhereCondition("    ", firstKeyColumn);
				writer.WriteLine();

				foreach (Column column in middleKeyColumns)
				{
					WriteWhereCondition("AND ", column);
					writer.WriteLine();
				}

				WriteWhereCondition("AND ", lastKeyColumn);
			}

			writer.WriteLine("\";");

			writer.WriteLine();

			writer.WriteLine("        var param = new");
			writer.WriteLine("        {");

			foreach (Column keyColumn in keyColumns)
			{
				writer.Write("            ");
				writer.Write(keyColumn.ColumnName.SnakeCase);
				writer.Write(" = ");
				writer.Write(keyColumn.ColumnName.CamelCase);
				writer.WriteLine(",");
			}

			writer.WriteLine("        };");

			writer.WriteLine();

			writer.Write("        ");
			writer.Write(dataClassName);
			writer.Write(" result = connection.QuerySingle<");
			writer.Write(dataClassName);
			writer.WriteLine(">(sql, param);");

			writer.WriteLine();

			writer.WriteLine("        return result;");

			writer.WriteLine("    }");
		}

		// Blank line: Method - Method
		writer.WriteLine();

		// Methods: QueryAsync method
		{
			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.Write("    /// ");
			writer.Write(command.Description);
			writer.WriteLine("概要を問い合わせします。");
			writer.WriteLine("    /// </summary>");

			WriteDocumentationCommentParameter(keyColumns);

			writer.Write("    /// <returns>問い合わせした");
			writer.Write(command.Description);
			writer.WriteLine("概要データのコレクションを返します。</returns>");

			// Definition
			writer.Write("    public async Task<");
			writer.Write(dataClassName);
			writer.Write("> QuerySingleAsync(");

			// Parameter
			WriteMethodParameter(firstKeyColumn);
			foreach (Column keyColumn in secondAfterKeyColumns)
			{
				WriteMethodParameter(keyColumn);
			}

			writer.WriteLine(")");

			writer.WriteLine("    {");
			writer.Write("        return await Task.Run(() => QuerySingle(");

			writer.Write(firstKeyColumn.ColumnName.CamelCase);
			foreach (Column keyColumn in secondAfterKeyColumns)
			{
				writer.Write(", ");
				writer.Write(keyColumn.ColumnName.CamelCase);
			}

			writer.WriteLine(");");
			writer.WriteLine("    }");
		}

		// Methods: End
		writer.WriteLine();
		writer.WriteLine("    #endregion");

		writer.WriteLine("}");

		writer.Flush();
		stream.Position = 0;

		GenerationResult result;
		{
			string fileName = className + ".cs";

			result = new GenerationResult(fileName, stream);
		}

		return result;
	}

	/// <summary>
	/// クエリサービスのクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public async Task<GenerationResult> GenerateQueryServiceClassAsync(TableGenerationCommand command)
	{
		return await Task.Run(() => GenerateQueryServiceClass(command));
	}

	/// <summary>
	/// クエリサービスのインターフェースを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public GenerationResult GenerateQueryServiceInterface(TableGenerationCommand command)
	{
		string namespaceSequence = command.NamespaceSequence + ".Application";

		string interfaceName;
		string dataClassName;
		{
			TableName tableName = new(command.SingularName);

			interfaceName = $"I{tableName.PascalCase}QueryService";
			dataClassName = tableName.PascalCase + "Data";
		}

		Column[] keyColumns = command.Columns.Where(x => x.IsPrimaryKey).Select(x => new Column(new ColumnName(x.ColumnName), x.ColumnType, x.Description, x.IsNotNull, x.IsPrimaryKey, x.ContainsSummary)).ToArray();
		Column firstKeyColumn = keyColumns.First();
		Column[] secondAfterKeyColumns = keyColumns.Skip(1).ToArray();

		MemoryStream stream = new();

		StreamWriter writer = new(stream, Encoding.UTF8);

		// Using
		writer.Write("using ");
		writer.Write(namespaceSequence);
		writer.WriteLine(".Data;");

		// Blank line: Using - Namespace
		writer.WriteLine();

		// Namespace
		writer.Write("namespace ");
		writer.Write(namespaceSequence);
		writer.WriteLine(";");

		// Blank line: Namespace - Interface
		writer.WriteLine();

		// Class: Documentation comment
		writer.WriteLine("/// <summary>");
		writer.Write("/// ");
		writer.Write(command.Description);
		writer.WriteLine("の問い合わせサービス");
		writer.WriteLine("/// </summary>");

		// Class: Definition
		writer.Write("public interface ");
		writer.WriteLine(interfaceName);
		writer.WriteLine("{");

		// Methods: Region: Begin
		writer.WriteLine("    #region Methods");
		writer.WriteLine();

		void WriteDocumentationCommentParameter(Column[] keyColumns)
		{
			foreach (Column column in keyColumns)
			{
				writer.Write("    /// <param name=\"");
				writer.Write(column.ColumnName.CamelCase);
				writer.Write("\">");
				writer.Write(column.Description);
				writer.WriteLine("</param>");
			}
		}

		void WriteMethodParameter(Column column)
		{
			switch (column.Type)
			{
				case ColumnTypes.String:
					writer.Write("string");
					break;
				case ColumnTypes.Integer:
					writer.Write("int");
					break;
			}

			writer.Write(" ");
			writer.Write(column.ColumnName.CamelCase);
		}

		// Methods: Query method
		{
			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.Write("    /// ");
			writer.Write(command.Description);
			writer.WriteLine("を問い合わせします。");
			writer.WriteLine("    /// </summary>");

			WriteDocumentationCommentParameter(keyColumns);

			writer.Write("    /// <returns>問い合わせした");
			writer.Write(command.Description);
			writer.WriteLine("データを返します。</returns>");

			// Definition
			writer.Write("    ");
			writer.Write(dataClassName);
			writer.Write(" QuerySingle(");

			WriteMethodParameter(firstKeyColumn);
			foreach (Column column in secondAfterKeyColumns)
			{
				writer.Write(", ");
				WriteMethodParameter(column);
			}

			writer.WriteLine(");");
		}

		// Blank line: Method - Method
		writer.WriteLine();

		// Methods: QueryAsync method
		{
			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.Write("    /// ");
			writer.Write(command.Description);
			writer.WriteLine("を問い合わせします。");
			writer.WriteLine("    /// </summary>");

			WriteDocumentationCommentParameter(keyColumns);

			writer.Write("    /// <returns>問い合わせした");
			writer.Write(command.Description);
			writer.WriteLine("データを返します。</returns>");

			// Definition
			writer.Write("    Task<");
			writer.Write(dataClassName);
			writer.Write("> QuerySingleAsync(");

			WriteMethodParameter(firstKeyColumn);
			foreach (Column column in secondAfterKeyColumns)
			{
				writer.Write(", ");
				WriteMethodParameter(column);
			}

			writer.WriteLine(");");
		}

		// Methods: Region: End
		writer.WriteLine();
		writer.WriteLine("    #endregion");

		writer.WriteLine("}");

		writer.Flush();
		stream.Position = 0;

		GenerationResult result;
		{
			string fileName = interfaceName + ".cs";

			result = new GenerationResult(fileName, stream);
		}

		return result;
	}

	/// <summary>
	/// クエリサービスのインターフェースを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public async Task<GenerationResult> GenerateQueryServiceInterfaceAsync(TableGenerationCommand command)
	{
		return await Task.Run(() => GenerateQueryServiceInterface(command));
	}

	/// <summary>
	/// 概要データクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public GenerationResult GenerateSummaryDataClass(TableGenerationCommand command)
	{
		string className;
		{
			TableName tableName = new(command.SingularName);
			className = tableName.PascalCase + "SummaryData";
		}

		Column[] columns = command.Columns.Where(x => x.ContainsSummary).Select(x => new Column(new ColumnName(x.ColumnName), x.ColumnType, x.Description, x.IsNotNull, x.IsPrimaryKey, x.ContainsSummary)).ToArray();
		Column firstColumn = columns.First();
		Column[] secondAfterColumns = columns.Skip(1).ToArray();

		MemoryStream stream = new();

		StreamWriter writer = new(stream, Encoding.UTF8);

		// Namespace
		writer.Write("namespace ");
		writer.Write(command.NamespaceSequence);
		writer.WriteLine(".Application.Data;");

		writer.WriteLine();

		// Class - Documentation comment
		writer.WriteLine("/// <summary>");
		writer.Write("/// ");
		writer.Write(command.Description);
		writer.WriteLine("概要データ");
		writer.WriteLine("/// </summary>");

		foreach (Column column in columns)
		{
			writer.Write("/// <param name=\"");
			writer.Write(column.ColumnName.PascalCase);
			writer.Write("\">");
			writer.Write(column.Description);
			writer.WriteLine("</param>");
		}

		// Class - Begin: Definition
		writer.Write("public record class ");
		writer.Write(className);
		writer.Write("(");

		void writeProperty(Column column)
		{
			switch (column.Type)
			{
				case ColumnTypes.String:
					writer.Write("string");
					break;
				case ColumnTypes.Integer:
					writer.Write("int");
					break;
			}

			if (!column.IsNotNull)
			{
				writer.Write("?");
			}

			writer.Write(" ");
			writer.Write(column.ColumnName.PascalCase);
		}

		writeProperty(firstColumn);
		foreach (Column column in secondAfterColumns)
		{
			writer.Write(", ");
			writeProperty(column);
		}

		writer.WriteLine(");");

		writer.Flush();
		stream.Position = 0;

		GenerationResult result;
		{
			string fileName = className + ".cs";

			result = new GenerationResult(fileName, stream);
		}

		return result;
	}

	/// <summary>
	/// 概要データクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public async Task<GenerationResult> GenerateSummaryDataClassAsync(TableGenerationCommand command)
	{
		return await Task.Run(() => GenerateSummaryDataClass(command));
	}

	/// <summary>
	/// 概要クエリサービスのクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public GenerationResult GenerateSummaryQueryServiceClass(TableGenerationCommand command)
	{
		string namespaceSequence = command.NamespaceSequence + ".Application";

		string className;
		string interfaceName;
		string dataClassName;
		{
			TableName tableName = new(command.SingularName);

			className = tableName.PascalCase + "SummaryQueryService";
			interfaceName = "I" + className;
			dataClassName = tableName.PascalCase + "SummaryData";
		}

		Column[] columns = command.Columns.Select(x => new Column(new ColumnName(x.ColumnName), x.ColumnType, x.Description, x.IsNotNull, x.IsPrimaryKey, x.ContainsSummary)).ToArray();

		Column[] summaryColumns = columns.Where(x => x.ContainsSummary).ToArray();
		Column firstSummaryColumn = summaryColumns.First();
		Column[] secondAfterSummaryColumns = summaryColumns.Skip(1).ToArray();

		Column[] keyColumns = columns.Where(x => x.IsPrimaryKey).ToArray();
		Column firstKeyColumn = keyColumns.First();
		Column[] secondAfterKeyColumns = keyColumns.Skip(1).ToArray();

		MemoryStream stream = new();

		StreamWriter writer = new(stream, Encoding.UTF8);

		// Using
		{
			List<string> references = [
				"Dapper",
				"Microsoft.Extensions.Configuration",
				namespaceSequence + ".Data",
			];

			references.Sort();

			foreach (string reference in references)
			{
				writer.Write("using ");
				writer.Write(reference);
				writer.WriteLine(";");
			}
		}

		// Blank line: Using - Namespace
		writer.WriteLine();

		// Namespace
		writer.Write("namespace ");
		writer.Write(namespaceSequence);
		writer.WriteLine(".Sqlite;");

		// Blank line: Namespace - Interface
		writer.WriteLine();

		// Class: Documentation comment
		writer.WriteLine("/// <summary>");
		writer.Write("/// ");
		writer.Write(command.Description);
		writer.WriteLine("概要の問い合わせサービス");
		writer.WriteLine("/// </summary>");
		writer.WriteLine("/// <param name=\"configuration\">設定</param>");

		// Class: Definition
		writer.Write("public class ");
		writer.Write(className);
		writer.Write("(IConfiguration configuration) : ");
		writer.WriteLine(interfaceName);
		writer.WriteLine("{");

		// Fields: Begin
		writer.WriteLine("    #region Fields");
		writer.WriteLine();

		// Field: _connectionString
		writer.WriteLine("    /// <summary>");
		writer.WriteLine("    /// 接続文字列");
		writer.WriteLine("    /// </summary>");
		writer.Write("    private readonly string _connectionString = configuration.GetConnectionString(\"");
		writer.Write(command.ConnectionStringKey);
		writer.WriteLine("\") ?? throw new InvalidOperationException(\"接続文字列が取得できません。\");");

		// Fields: End
		writer.WriteLine();
		writer.WriteLine("    #endregion");

		// Blank line: Fields - Methods
		writer.WriteLine();

		// Methods: Begin
		writer.WriteLine("    #region Methods");
		writer.WriteLine();

		// Methods: Query method
		{
			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.Write("    /// ");
			writer.Write(command.Description);
			writer.WriteLine("概要を問い合わせします。");
			writer.WriteLine("    /// </summary>");
			writer.Write("    /// <returns>問い合わせした");
			writer.Write(command.Description);
			writer.WriteLine("概要データのコレクションを返します。</returns>");

			// Definition
			writer.Write("    public IEnumerable<");
			writer.Write(dataClassName);
			writer.WriteLine("> Query()");

			writer.WriteLine("    {");

			writer.WriteLine("        using SqliteConnection connection = new(_connectionString);");
			writer.WriteLine("        connection.Open();");

			writer.WriteLine();

			writer.WriteLine("        const string sql = @\"SELECT");

			void WriteColumn(string separator, Column column)
			{
				writer.Write("    ");

				if (separator != string.Empty)
				{
					writer.Write(separator);
				}

				writer.Write(column.ColumnName.SnakeCase);
			}

			if (columns.Length > 1)
			{
				WriteColumn("  ", firstSummaryColumn);
				writer.WriteLine();

				foreach (Column column in secondAfterSummaryColumns)
				{
					WriteColumn(", ", column);
					writer.WriteLine();
				}
			}
			else if (columns.Length == 1)
			{
				WriteColumn(string.Empty, firstSummaryColumn);
			}

			writer.WriteLine("FROM ");
			writer.Write("    ");
			writer.WriteLine(command.PluralName);
			writer.WriteLine("ORDER BY");

			if (keyColumns.Length == 1)
			{
				WriteColumn(string.Empty, firstKeyColumn);
			}
			else if (keyColumns.Length > 1)
			{
				WriteColumn("  ", firstKeyColumn);
				writer.WriteLine();

				Column[] middleKeyColumns = secondAfterKeyColumns.SkipLast(1).ToArray();
				Column lastKeyColumn = secondAfterKeyColumns.Last();
				foreach (Column column in middleKeyColumns)
				{
					WriteColumn(", ", column);
					writer.WriteLine();
				}

				WriteColumn(", ", lastKeyColumn);
			}

			writer.WriteLine("\";");

			writer.WriteLine();

			writer.Write("        ");
			writer.Write(dataClassName);
			writer.Write("[] results = connection.Query<");
			writer.Write(dataClassName);
			writer.WriteLine(">(sql).ToArray();");

			writer.WriteLine();

			writer.WriteLine("        return results;");

			writer.WriteLine("    }");
		}

		// Blank line: Method - Method
		writer.WriteLine();

		// Methods: QueryAsync method
		{
			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.Write("    /// ");
			writer.Write(command.Description);
			writer.WriteLine("概要を問い合わせします。");
			writer.WriteLine("    /// </summary>");
			writer.Write("    /// <returns>問い合わせした");
			writer.Write(command.Description);
			writer.WriteLine("概要データのコレクションを返します。</returns>");

			// Definition
			writer.Write("    public async Task<IEnumerable<");
			writer.Write(dataClassName);
			writer.WriteLine(">> QueryAsync()");

			writer.WriteLine("    {");
			writer.WriteLine("        return await Task.Run(Query);");
			writer.WriteLine("    }");
		}

		// Methods: End
		writer.WriteLine();
		writer.WriteLine("    #endregion");

		writer.WriteLine("}");

		writer.Flush();
		stream.Position = 0;

		GenerationResult result;
		{
			string fileName = className + ".cs";

			result = new GenerationResult(fileName, stream);
		}

		return result;
	}

	/// <summary>
	/// 概要クエリサービスのクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public async Task<GenerationResult> GenerateSummaryQueryServiceClassAsync(TableGenerationCommand command)
	{
		return await Task.Run(() => GenerateSummaryQueryServiceClass(command));
	}

	/// <summary>
	/// 概要クエリサービスのインターフェースを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public GenerationResult GenerateSummaryQueryServiceInterface(TableGenerationCommand command)
	{
		string namespaceSequence = command.NamespaceSequence + ".Application";

		string interfaceName;
		string dataClassName;
		{
			TableName tableName = new(command.SingularName);

			interfaceName = $"I{tableName.PascalCase}SummaryQueryService";
			dataClassName = tableName.PascalCase + "SummaryData";
		}

		MemoryStream stream = new();

		StreamWriter writer = new(stream, Encoding.UTF8);

		// Using
		writer.Write("using ");
		writer.Write(namespaceSequence);
		writer.WriteLine(".Data;");

		// Blank line: Using - Namespace
		writer.WriteLine();

		// Namespace
		writer.Write("namespace ");
		writer.Write(namespaceSequence);
		writer.WriteLine(";");

		// Blank line: Namespace - Interface
		writer.WriteLine();

		// Class: Documentation comment
		writer.WriteLine("/// <summary>");
		writer.Write("/// ");
		writer.Write(command.Description);
		writer.WriteLine("概要の問い合わせサービス");
		writer.WriteLine("/// </summary>");

		// Class: Definition
		writer.Write("public interface ");
		writer.WriteLine(interfaceName);
		writer.WriteLine("{");

		// Methods: Region: Begin
		writer.WriteLine("    #region Methods");
		writer.WriteLine();

		// Methods: Query method
		{
			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.Write("    /// ");
			writer.Write(command.Description);
			writer.WriteLine("概要を問い合わせします。");
			writer.WriteLine("    /// </summary>");
			writer.Write("    /// <returns>問い合わせした");
			writer.Write(command.Description);
			writer.WriteLine("概要データのコレクションを返します。</returns>");

			// Definition
			writer.Write("    IEnumerable<");
			writer.Write(dataClassName);
			writer.WriteLine("> Query();");
		}

		// Blank line: Method - Method
		writer.WriteLine();

		// Methods: QueryAsync method
		{
			// Documentation comment
			writer.WriteLine("    /// <summary>");
			writer.Write("    /// ");
			writer.Write(command.Description);
			writer.WriteLine("概要を問い合わせします。");
			writer.WriteLine("    /// </summary>");
			writer.Write("    /// <returns>問い合わせした");
			writer.Write(command.Description);
			writer.WriteLine("概要データのコレクションを返します。</returns>");

			// Definition
			writer.Write("    Task<IEnumerable<");
			writer.Write(dataClassName);
			writer.WriteLine(">> QueryAsync();");
		}

		// Methods: Region: End
		writer.WriteLine();
		writer.WriteLine("    #endregion");

		writer.WriteLine("}");

		writer.Flush();
		stream.Position = 0;

		GenerationResult result;
		{
			string fileName = interfaceName + ".cs";

			result = new GenerationResult(fileName, stream);
		}

		return result;
	}

	/// <summary>
	/// 概要クエリサービスのインターフェースを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	public async Task<GenerationResult> GenerateSummaryQueryServiceInterfaceAsync(TableGenerationCommand command)
	{
		return await Task.Run(() => GenerateSummaryQueryServiceInterface(command));
	}

	#endregion
}
