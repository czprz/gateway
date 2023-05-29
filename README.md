# Gateway

Reverse proxy gateway using Yarp with the logic to pass/exchange token for upstream services and API for registering routes

## Getting Started

```
git clone https://github.com/czprz/gateway.git
```

### Prerequisites

You'll need to install below dependencies

```
.NET 7 SDK
```

### Swagger

Swagger is integrated and available when running as debug. 

http://localhost/swagger/index.html


### Running the tests

Currently no tests implemented..


### Running

```
docker-compose up
```

### Built With

* [.NET 7](https://dotnet.microsoft.com/en-us/)
* [Yarp](https://microsoft.github.io/reverse-proxy/)
* [Keycloak](https://www.keycloak.org/)

### Made possible with these libraries

* [Asp.Versioning.Http](https://www.nuget.org/packages/Asp.Versioning.Http)
* [Asp.Versioning.Mvc.ApiExplorer](https://www.nuget.org/packages/Asp.Versioning.Mvc.ApiExplorer)
* [Microsoft.AspNetCore.OpenApi](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi)
* [Swashbuckle.AspNetCore](https://www.nuget.org/packages?q=Swashbuckle.AspNetCore)

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting a pull
request.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see
the [releases on this repository](https://github.com/czprz/gateway/releases).

## Authors

* **[Casper Overholm Elkrog](https://github.com/czprz)**

See also the list of [contributors](https://github.com/czprz/gateway/network/) who participated in this project.

## License

This project is licensed under the The Unlicense - see the [LICENSE](LICENSE) file for details
