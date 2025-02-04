using System.Text;
using SourceCodeGeneration.Application.Command;

namespace SourceCodeGeneration.Application;

/// <summary>
/// 不変オブジェクト生成サービス
/// </summary>
public class ImmutableObjectGenerationService : IImmutableObjectGenerationService
{
    #region Methods

    /// <summary>
    /// 不変オブジェクトを生成します。
    /// </summary>
    /// <param name="command">コマンド</param>
    /// <returns>生成したストリームを返します。</returns>
    public Stream GenerateImmutableObject(ImmutableObjectGenerationCommand command)
    {
		ImmutableObjectPropertyCommand firstProperty = command.Properties[0];
        IReadOnlyCollection<ImmutableObjectPropertyCommand> secondAndSubsequentProperties = command.Properties.Skip(1).ToList();

        MemoryStream stream = new();

        StreamWriter writer = new(stream, Encoding.UTF8);

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
        writer.Write(command.ClassName);
        writer.Write("(");

        static void writeParameter(StreamWriter writer, ImmutableObjectPropertyCommand property)
        {
			writer.Write(property.TypeName);
			writer.Write(" ");
			writer.Write(property.PropertyName);
		}

        writeParameter(writer, firstProperty);
		foreach (ImmutableObjectPropertyCommand property in secondAndSubsequentProperties)
		{
			writer.Write(", ");
			writeParameter(writer, property);
		}

        writer.WriteLine(");");

        writer.Flush();

        return stream;
    }

    /// <summary>
    /// 不変オブジェクトを生成します。
    /// </summary>
    /// <param name="command">コマンド</param>
    /// <returns>生成したストリームを返します。</returns>
    public async Task<Stream> GenerateImmutableObjectAsync(ImmutableObjectGenerationCommand command)
    {
        return await Task.Run(() => GenerateImmutableObject(command));
    }

    #endregion
}
