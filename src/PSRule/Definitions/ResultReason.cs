// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Threading;
using PSRule.Runtime;

namespace PSRule.Definitions
{
    internal sealed class ResultReason : IResultReasonV2
    {
        private string _Formatted;
        private string _Message;
        private readonly IOperand _Operand;

        internal ResultReason(string parentPath, IOperand operand, string text, object[] args)
        {
            _Operand = operand;
            Text = text;
            Args = args;
            FullPath = ObjectPathJoin(parentPath, operand?.Path);
        }

        /// <summary>
        /// The object path that failed.
        /// </summary>
        public string Path => _Operand?.Path;

        /// <summary>
        /// The object path including the path of the parent object.
        /// </summary>
        public string FullPath { get; }

        public string Text { get; }

        public object[] Args { get; }

        public string Message
        {
            get
            {
                _Message ??= Args == null || Args.Length == 0 ? Text : string.Format(Thread.CurrentThread.CurrentCulture, Text, Args);
                return _Message;
            }
        }

        public override string ToString()
        {
            return Format();
        }

        public string Format()
        {
            _Formatted ??= string.Concat(
                _Operand?.ToString(),
                Message
            );
            return _Formatted;
        }

        private string ObjectPathJoin(string parentPath, string path)
        {
            if (string.IsNullOrEmpty(parentPath))
                return path;

            return string.IsNullOrEmpty(path) ? parentPath : string.Concat(parentPath, ".", path);
        }
    }
}