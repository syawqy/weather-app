# Weather Web App

This is a simple web application built with .NET Core that allows users to look up the current weather for a given city.

## Features

- Look up current weather for a city
- View temperature, humidity, wind speed, description (i.e. sunny, cloudy, etc.) 
- Search by city name

## Technologies Used

- .NET Core 3.1
- OpenWeatherMap API

## Getting Started

1. Clone the repository

```
git clone https://github.com/syawqy/weather-app.git
```

2. Navigate to project folder

```
cd weather-app
```

3. Add API key from openweathermap http://api.openweathermap.org/ and add it to appsettings.json

```
"WeatherAPIKey": "YOURAPIKEY",
```

4. Build and run the project

```
dotnet build
dotnet run
```

5. Open a browser to view the web app


## Usage

On the home page, choose a country then choose a city name, the weather information will be showed quickly after that.


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for more details.

Let me know if you would like any changes to this README!
