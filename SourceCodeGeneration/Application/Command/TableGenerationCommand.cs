namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// テーブルの生成コマンド
/// </summary>
/// <param name="SingularName">単数形の名前</param>
/// <param name="PluralName">複数形の名前</param>
/// <param name="Description">説明</param>
/// <param name="Columns">列のコレクション</param>
/// <param name="NamespaceSequence">名前空間のシーケンス</param>
/// <param name="ConnectionStringKey">接続文字列のキー</param>
public record class TableGenerationCommand(string SingularName, string PluralName, string Description, IReadOnlyCollection<TableColumnGenerationCommand> Columns, string NamespaceSequence, string ConnectionStringKey);
