using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MovieExplorer.Core.UnitTests {
	[TestFixture]
	public class UrlHelper {
		[Test]
		public void AppendPath_ValidInput_ReturnsUrlWithAppendedPath() {
			string url = "https://wolfprogrammer.com";
			string path = "test/path/";
			string expected = "https://wolfprogrammer.com/test/path";
			string actual = Core.UrlHelper.AppendPath(url, path);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendPath_ForNullUrlAndPath_ReturnsNull() {
			string url = null;
			string path = null;
			string expected = null;
			string actual = Core.UrlHelper.AppendPath(url, path);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendPath_ForNullUrl_ReturnsNull() {
			string url = null;
			string path = "/test/path/";
			string expected = null;
			string actual = Core.UrlHelper.AppendPath(url, path);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendPath_ForNullPath_ReturnsOriginalUrl() {
			string url = "https://wolfprogrammer.com/";
			string path = null;
			string expected = "https://wolfprogrammer.com/";
			string actual = Core.UrlHelper.AppendPath(url, path);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendPath_DuplicateSlashes_ReturnsUrlWithAppendedPath() {
			string url = "https://wolfprogrammer.com/";
			string path = "/path/";
			string expected = "https://wolfprogrammer.com/path";
			string actual = Core.UrlHelper.AppendPath(url, path);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendPath_WithSpaces_ReturnsUrlWithAppendedPath() {
			string url = "   https://wolfp  rogrammer.com  ";
			string path = " pa th ";
			string expected = "https://wolfprogrammer.com/path";
			string actual = Core.UrlHelper.AppendPath(url, path);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendQuerystring_ValidInput_ReturnsUrlWithAppendedQuerystring() {
			string url = "https://wolfprogrammer.com";
			Dictionary<string, string> parameters = 
				new Dictionary<string, string> { { "Id", "12345" } };
			string expected = "https://wolfprogrammer.com/?Id=12345";
			string actual = Core.UrlHelper.AppendQuerystring(url, parameters);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendQuerystring_ForNullUrlAndParameters_ReturnsNull() {
			string url = null;
			Dictionary<string,string> parameters = null;
			string expected = null;
			string actual = Core.UrlHelper.AppendQuerystring(url, parameters);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendQuerystring_ForNullUrl_ReturnsNull() {
			string url = null;
			Dictionary<string, string> parameters = 
				new Dictionary<string, string>{ { "Id", "12345" } };
			string expected = null;
			string actual = Core.UrlHelper.AppendQuerystring(url, parameters);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendQuerystring_ForNullParameters_ReturnsOriginalUrl() {
			string url = "https://wolfprogrammer.com";
			Dictionary<string, string> parameters = null;
			string expected = "https://wolfprogrammer.com";
			string actual = Core.UrlHelper.AppendQuerystring(url, parameters);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendQuerystring_ForQueryKeyWithSpecialCharsInKey_ReturnsUrlWithAppendedParameters() {
			string url = "http://wolfprogrammer.com";
			Dictionary<string, string> parameters = 
				new Dictionary<string, string> { { "test@#$%^&3", "asdf" } };
			string expected = "http://wolfprogrammer.com/?test%40%23%24%25%5E%263=asdf";
			string actual = Core.UrlHelper.AppendQuerystring(url, parameters);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendQuerystring_ForQueryKeyWithSpecialCharsInValue_ReturnsUrlWithAppendedParameters() {
			string url = "http://wolfprogrammer.com";
			Dictionary<string, string> parameters = 
				new Dictionary<string, string> { { "test", "asdf~!@#$%^&*()3" } };
			string expected = "http://wolfprogrammer.com/?test=asdf~!%40%23%24%25%5E%26*()3";
			string actual = Core.UrlHelper.AppendQuerystring(url, parameters);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendQuerystring_MultipleParameters_ReturnsUrlWithAppendedParameters() {
			string url = "http://wolfprogrammer.com";
			Dictionary<string, string> parameters = new Dictionary<string, string> {
				{ "key1", "value1" },
				{ "key2", "value2" },
				{ "key3", "value3" },
			};
			string expected = "http://wolfprogrammer.com/?key1=value1&key2=value2&key3=value3";
			string actual = Core.UrlHelper.AppendQuerystring(url, parameters);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void AppendQuerystring_ForEmptyParameters_ReturnsOriginalUrl() {
			string url = "http://wolfprogrammer.com";
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			string expected = "http://wolfprogrammer.com";
			string actual = Core.UrlHelper.AppendQuerystring(url, parameters);
			Assert.AreEqual(expected, actual);
		}
	}
}
