using AngleSharp.Dom;
using BenchmarkDotNet.Attributes;

namespace AngleSharp.Benchmarks
{
    [MemoryDiagnoser, ShortRunJob]
    public class UrlBenchmark
    {
        private const int Iterations = 25;

        private static readonly string[] _urls =
        {
            "http://localhost:8080/?mytest",
            "https://www.google.de/mypath",
            "http://www.google.de",
            "http://example.org/foo/bar",
            "http://example-domain.com/image.jpg",
            "https://github.com/FlorianRappl/AngleSharp",
            "http://www.google.de/some-path?a=b#header",
            "http://test/?hi—there",
            "http://example_domain.com/image.jpg",
            "https://loony_picture.dirty.ru/",
            "http://localhost:12345/account/login",
            "http://www.google.de/some-path#",
            "https://api.example.com/v2/users?page=1&limit=50",
            "http://192.168.1.1:3000/dashboard",
            "https://cdn.jsdelivr.net/npm/anglesharp@1.0.0/dist/index.min.js",
            "ftp://files.example.com/pub/documents/report.pdf",
            "https://example.com/path/to/resource?q=hello+world&lang=en#section-2",
            "http://subdomain.deep.nested.example.co.uk/page",
            "https://user:password@secure.example.com:8443/login",
            "http://example.com/path%20with%20spaces/file%2Fname.html",
            "https://www.amazon.com/dp/B08N5WRWNW?ref=cm_sw_r_cp_ud_dp",
            "http://localhost/",
            "https://example.com:443/",
            "http://example.com:80/default",
            "https://maps.google.com/maps?q=48.8566,2.3522&z=15",
            "http://例え.jp/",
            "https://www.example.com/search?q=angle+sharp&category=lib&sort=relevance",
            "http://test.com/a/b/c/d/e/f/g/h/i/j/k/l/m/n/o/p",
            "https://example.com/?",
            "http://example.com/#",
            "https://example.com/path?key=value&key=other",
            "http://www.example.com:65535/max-port",
            "https://example.com/unicode/café/naïve",
            "http://example.com/path?a=1&b=2&c=3&d=4&e=5&f=6",
            "https://raw.githubusercontent.com/nicholaspark09/awesome/master/README.md",
            "http://www.bbc.co.uk/news/world-europe-12345678",
            "https://stackoverflow.com/questions/12345/how-to-parse-urls",
            "http://example.com/image.png?width=800&height=600&format=webp",
            "https://accounts.google.com/o/oauth2/v2/auth?client_id=abc&redirect_uri=http%3A%2F%2Flocalhost",
            "http://example.com/path/../normalized",
            "https://example.com/trailing-slash/",
            "http://example.com/no-trailing-slash",
            "https://www.youtube.com/watch?v=dQw4w9WgXcQ&list=PLrAXtmErZgOeiKm4sgNOknGvNjby9efdf",
            "http://example.com:8080/api/v1/items/42?fields=name,price&format=json",
            "https://example.com/path#fragment-with-special/chars?not=query",
            "http://[::1]:8080/ipv6-localhost",
            "https://example.com/.hidden/file",
            "http://example.com/file.tar.gz",
            "https://example.com/api?callback=jQuery_1234&_=1617890",
            "http://example.com/very-long-path/segment1/segment2/segment3/segment4/segment5/file.html",
            "https://en.wikipedia.org/wiki/Percent-encoding#Types_of_URI_characters",
            "http://10.0.0.1/admin/config",
            "https://registry.npmjs.org/@anthropic-ai/sdk/-/sdk-1.0.0.tgz",
            "http://example.com/path?utm_source=google&utm_medium=cpc&utm_campaign=spring_sale&utm_term=shoes",
            "https://api.stripe.com/v1/charges?limit=3&starting_after=ch_abc123",
            "http://example.com/data.json?jsonp=parseResponse",
            "https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap",
            "http://example.com/download/file%20(1).zip",
            "https://gitlab.com/group/subgroup/project/-/merge_requests/42",
            "http://example.com:9200/_search?q=title:anglesharp&size=10",
            "https://example.com/api/v3/repos/owner/repo/issues?state=open&labels=bug",
            "http://[2001:db8:85a3::8a2e:370:7334]/ipv6-full",
            "https://example.com/path/to/page?redirect=https%3A%2F%2Fother.com%2Fpage",
            "http://example.com/~user/public_html/index.html",
            "https://www.example.com/products/category/subcategory/item-name-slug-12345",
            "http://example.com/api?filter[name]=test&filter[status]=active",
            "https://hooks.example.com/services/T00000000/B00000000/XXXXXXXXXXXXXXXXXXXXXXXX",
            "http://example.com/path;params?query=value",
            "https://example.com/data:text/html,<h1>Hello</h1>",
            "http://example.com/.well-known/acme-challenge/token123",
            "https://s3.amazonaws.com/bucket-name/key/with/slashes/object.dat",
            "http://example.com/path?empty_value=&another=test",
            "https://example.com/a?b=1#c=2&d=3",
            "http://www.example.com/path/./to/../resource",
            "https://example.com/CaseSensitive/PATH/file.HTML",
            "http://example.com:1/min-port",
            "https://example.com/path?q=%E4%B8%AD%E6%96%87",
            "http://example.com/foo?bar=baz&qux=quux&corge=grault&garply=waldo",
            "https://cdn.example.com/assets/css/main.min.css?v=2.3.1",
            "http://example.com/rss.xml",
            "https://example.com/sitemap.xml.gz",
            "http://admin:secret@example.com/protected/resource",
            "https://example.com/api/graphql?query={user(id:1){name,email}}",
            "http://example.com/path/with/trailing/slash/",
            "https://example.com/file.php?id=42&action=download&token=abc123def456",
            "http://172.16.254.1:8888/metrics",
            "https://example.com/v1/organizations/org-123/projects/proj-456/resources",
            "http://example.com/?q=a%26b%3Dc",
            "https://ws.example.com/socket?session_id=xyz789",
            "http://example.com/path?ts=1617235200&sig=sha256-abcdef1234567890",
            "https://example.com/doc#heading-1",
            "http://example.com/multi?a[]=1&a[]=2&a[]=3",
            "https://example.com/page.html?lang=en-US&region=NA&theme=dark&debug=false",
            "http://example.com/assets/fonts/OpenSans-Regular.woff2",
            "https://example.com/callback?code=AUTH_CODE_HERE&state=random_state_value",
            "http://example.com:8080/long/query?a=1&b=2&c=3&d=4&e=5&f=6&g=7&h=8&i=9&j=10",
            "https://example.com/embed?url=http://other.example.com/page&autoplay=1",
            "http://example.com/api/v2/search?q=hello&page=3&per_page=25&sort=updated&order=desc",
            "https://example.com/deep/nested/path/to/a/very/specific/resource/item.json",
        };

        [Benchmark]
        public Url[] Parse()
        {
            var results = new Url[_urls.Length];

            for (var n = 0; n < Iterations; n++)
            {
                for (var i = 0; i < _urls.Length; i++)
                {
                    results[i] = new Url(_urls[i]);
                }
            }

            return results;
        }

        [Benchmark]
        public string[] Href()
        {
            var results = new string[_urls.Length];

            for (var n = 0; n < Iterations; n++)
            {
                for (var i = 0; i < _urls.Length; i++)
                {
                    results[i] = new Url(_urls[i]).Href;
                }
            }

            return results;
        }

        [Benchmark]
        public string[] Components()
        {
            var results = new string[_urls.Length];

            for (var n = 0; n < Iterations; n++)
            {
                for (var i = 0; i < _urls.Length; i++)
                {
                    var url = new Url(_urls[i]);
                    _ = url.Scheme;
                    _ = url.Host;
                    _ = url.Port;
                    _ = url.Path;
                    _ = url.Query;
                    _ = url.Fragment;
                    results[i] = url.Origin;
                }
            }

            return results;
        }
    }
}
