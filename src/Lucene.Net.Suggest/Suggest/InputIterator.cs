﻿using System.Collections.Generic;
using Lucene.Net.Search.Suggest.Analyzing;
using Lucene.Net.Util;

namespace Lucene.Net.Search.Suggest
{

    /*
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */

    /// <summary>
    /// Interface for enumerating term,weight,payload triples for suggester consumption;
    /// currently only <seealso cref="AnalyzingSuggester"/>, {@link
    /// FuzzySuggester} and <seealso cref="AnalyzingInfixSuggester"/> support payloads.
    /// </summary>
    public interface InputIterator : BytesRefIterator
    {

        /// <summary>
        /// A term's weight, higher numbers mean better suggestions. </summary>
        long Weight { get; }

        /// <summary>
        /// An arbitrary byte[] to record per suggestion.  See
        ///  <seealso cref="Lookup.LookupResult#payload"/> to retrieve the payload
        ///  for each suggestion. 
        /// </summary>
        BytesRef Payload { get; }

        /// <summary>
        /// Returns true if the iterator has payloads </summary>
        bool HasPayloads { get; }

        /// <summary>
        /// A term's contexts context can be used to filter suggestions.
        /// May return null, if suggest entries do not have any context
        /// </summary>
        HashSet<BytesRef> Contexts { get; }

        /// <summary>
        /// Returns true if the iterator has contexts </summary>
        bool HasContexts { get; }
    }

    /// <summary>
    /// Singleton InputIterator that iterates over 0 BytesRefs.
    /// </summary>
    public static class EmptyInputIterator
    {
        public static readonly InputIterator Instance = new InputIteratorWrapper(EmptyBytesRefIterator.Instance);
    }

    /// <summary>
    /// Wraps a BytesRefIterator as a suggester InputIterator, with all weights
    /// set to <code>1</code> and carries no payload
    /// </summary>
    public class InputIteratorWrapper : InputIterator
    {
        internal readonly BytesRefIterator wrapped;

        /// <summary>
        /// Creates a new wrapper, wrapping the specified iterator and 
        /// specifying a weight value of <code>1</code> for all terms 
        /// and nullifies associated payloads.
        /// </summary>
        public InputIteratorWrapper(BytesRefIterator wrapped)
        {
            this.wrapped = wrapped;
        }

        public virtual long Weight
        {
            get { return 1; }
        }

        public BytesRef Next()
        {
            return wrapped.Next();
        }

        public virtual BytesRef Payload
        {
            get { return null; }
        }

        public virtual bool HasPayloads
        {
            get { return false; }
        }

        public IComparer<BytesRef> Comparator
        {
            get
            {
                return wrapped.Comparator;
            }
        }

        public virtual HashSet<BytesRef> Contexts
        {
            get { return null; }
        }

        public virtual bool HasContexts
        {
            get { return false; }
        }
    }

}