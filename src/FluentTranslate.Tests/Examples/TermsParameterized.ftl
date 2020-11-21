# A contrived example to demonstrate how variables
# can be passed to terms.
-https = https://{ $host }
visit = Visit { -https(host: "example.com") } for more information.