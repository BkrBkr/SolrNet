﻿using System;
using Xunit;
using SolrNet.Impl;
using SolrNet.Impl.DocumentPropertyVisitors;
using SolrNet.Impl.FieldParsers;
using SolrNet.Impl.ResponseParsers;
using SolrNet.Mapping;
using SolrNet.Tests.Utils;
using TestDoc = SolrNet.Tests.SolrDocumentSerializerTests.TestDoc;
using Doc = SolrNet.Tests.SolrDocumentSerializerTests.TestDocWithLocation;

namespace SolrNet.Tests {
    
    public class DefaultResponseParserTests {
        [Fact]
        public void ParseResponseWithLocation() {
            var mapper = new AttributesMappingManager();
            var parser = new DefaultResponseParser<Doc>(new SolrDocumentResponseParser<Doc>(mapper, new DefaultDocumentVisitor(mapper, new DefaultFieldParser()), new SolrDocumentActivator<Doc>()));
            var xml = EmbeddedResource.GetEmbeddedXml(GetType(), "Resources.response.xml");
            var results = new SolrQueryResults<Doc>();
            parser.Parse(xml, results);
            Assert.Equal(1, results.Count);
            Assert.Equal(new Location(51.5171, -0.1062), results[0].Loc);
        }

        [Fact]
        public void Parse_If_Both_Result_And_Groups_Are_Present()
        {
            var mapper = new AttributesMappingManager();
            var parser = new DefaultResponseParser<TestDoc>(new SolrDocumentResponseParser<TestDoc>(mapper, new DefaultDocumentVisitor(mapper, new DefaultFieldParser()), new SolrDocumentActivator<TestDoc>()));
            var xml = EmbeddedResource.GetEmbeddedXml(GetType(), "Resources.responseWithResultAndGroup.xml");
            var results = new SolrQueryResults<TestDoc>();
            parser.Parse(xml, results);
            Assert.Equal(1, results.Count);
            Assert.Equal(1, results.Grouping["titleId"].Ngroups);
        }
    }
}
