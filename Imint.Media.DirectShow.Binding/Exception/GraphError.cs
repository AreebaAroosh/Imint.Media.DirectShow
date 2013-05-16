﻿// 
//  GraphError.cs
//  
//  Author:
//       Simon Mika <simon.mika@imint.se>
//  
//  Copyright (c) 2012-2013 Imint AB
// 
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//  * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//  * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//  the documentation and/or other materials provided with the distribution.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
using System;
using Error = Kean.Core.Error;
using Kean.Core.Extension;

namespace Imint.Media.DirectShow.Binding.Exception
{
    public class GraphError :
        Abstract
    {
        GraphError(Error.Level level, string title, string message, params string[] arguments) :
            base(level, title, message, arguments)
        {
        }
        public static void Check(int code)
        {
            switch (code)
            {
                case 0: // no error, do nothing
                case 1: // succeeded, do nothing
                    break;
                default:
                    string message = DirectShowLib.DsError.GetErrorText(code);
                    if (message.NotNull())
                        new GraphError(Error.Level.Debug, "DirectShow Error", message).Throw();
                    else
                        new GraphError(Error.Level.Debug, "DirectShow Error", "Not recognized error code " + code + ".").Throw();
                    break;
            }
        }
    }
}
