// Decompiled with JetBrains decompiler
// Type: S2HD.CommandLineArguments
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Extensions;
using System.Collections.Generic;

namespace S2HD;

internal class CommandLineArguments
{
  private readonly HashSet<char> _flags = new HashSet<char>();
  private readonly Dictionary<string, string[]> _options = new Dictionary<string, string[]>();
  private readonly string _commandLine;

  public CommandLineArguments(IEnumerable<string> args)
    : this(args.AsArray<string>())
  {
  }

  public CommandLineArguments(string[] args)
  {
    List<string> stringList = new List<string>();
    this._commandLine = string.Join(" ", args);
    for (int index = 0; index < args.Length; ++index)
    {
      string str1 = args[index];
      if (str1.StartsWith("--"))
      {
        string key = str1.Substring(2);
        stringList.Clear();
        for (++index; index < args.Length; ++index)
        {
          string str2 = args[index];
          if (str2.StartsWith("-"))
          {
            --index;
            break;
          }
          stringList.Add(str2);
        }
        this._options.Add(key, stringList.ToArray());
      }
      else if (str1.StartsWith("-"))
      {
        foreach (char ch in str1.Substring(1))
          this._flags.Add(ch);
      }
    }
  }

  public bool HasFlag(char c) => this._flags.Contains(c);

  public bool HasOption(string option) => this._options.ContainsKey(option);

  public string[] GetOptionValues(string option)
  {
    string[] optionValues;
    if (!this._options.TryGetValue(option, out optionValues))
      optionValues = new string[0];
    return optionValues;
  }

  public override string ToString() => this._commandLine;
}
