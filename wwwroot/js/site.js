const { useState, useEffect } = React;
function App() {

    const [countries, setCountries] = useState([]);
    const [selectedCountry, setSelectedCountry] = useState('');

    const [cities, setCities] = useState([]);
    const [selectedCity, setSelectedCity] = useState('');

    const [weather, setWeather] = useState(null);

    useEffect(() => {
        // Fetch list of countries from API
        fetchCountries();
    }, []);

    const fetchCountries = async () => {
        const response = await fetch('/api/weather/countries');
        const countries = await response.json();
        setCountries(countries);
    }

    const handleCountryChange = async (e) => {
        const country = e.target.value;
        setSelectedCountry(country);

        // Fetch list of cities for selected country
        const response = await fetch(`/api/weather/cities/${country}`);
        const cities = await response.json();
        setCities(cities);
    }

    const handleCityChange = async (e) => {
        const city = e.target.value;
        setSelectedCity(city);

        const response = await fetch(`/api/weather?city=${city}`);
        const weather = await response.json();
        setWeather(weather);
    }

    return (
        <div className="container mt-5">
            <div className="row">
                <div className="col">
                    <select
                        value={selectedCountry}
                        onChange={handleCountryChange}
                        className="form-select mb-3"
                    >
                        <option>Select Country</option>
                        {countries.map((country, index) => (
                            <option key={index}>{country}</option>
                        ))}
                    </select>

                    <select
                        value={selectedCity}
                        onChange={handleCityChange}
                        className="form-select mb-3"
                    >
                        <option>Select City</option>
                        {cities.map((city, index) => (
                            <option key={index}>{city}</option>
                        ))}
                    </select>
                </div>
            </div>

            {weather && (
                <div className="row">
                    <div className="col">
                        <WeatherData data={weather} />
                    </div>
                </div>
            )}
        </div>
    );
}

function WeatherData({ data }) {
    return (
        <div>
            <h2>Weather for {data.location}</h2>
            <p>Temperature (C°): {data.tempInC}</p>
            <p>Temperature (F°): {data.tempInF}</p>
            <p>Sky Condition: {data.skyCondition}</p>
            <p>Pressure: {data.pressure}</p>
            <p>Humidity: {data.relativeHumidity}</p>
            <p>Dew Point: {data.dewPoint}</p>
            <p>Visibility: {data.visibility}</p>
            <p>Wind: {data.wind}</p>
        </div>
    );
}


const rootElement = document.getElementById('root');
ReactDOM.render(<App />, rootElement);