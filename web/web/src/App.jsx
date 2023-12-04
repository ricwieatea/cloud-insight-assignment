import { useEffect, useState } from 'react'
import { Temperature } from './temperature/temperature'
import { Wind } from './wind/wind'
import './App.css'
import { GetWeatherDataFromApi } from './datasource'
import { Chart } from "react-google-charts";

function App() {
  // const [count, setCount] = useState(0)
  const [lowestTempPerCity, setLowestTempPerCity] = useState([]);
  const [higestWindspeedPerCity, setHigestWindspeedPerCity] = useState([]);

  useEffect(() => loadWeather, []);

  async function loadWeather() {
    const result = await GetWeatherDataFromApi();

    console.log('result', result);
    var tempData = result.temperatureData.map(x => {
      return [x.city, x.temperature_2m, `${x.city}, ${x.country} \n Temperature: ${x.temperature_2m} \n Synced at: ${ToShortDateTime(x.timestamp)}`]
    });

    var windData = result.windSpeedData.map(x => {
      return [x.city, x.wind_speed_10m, `${x.city}, ${x.country} \n Wind speed: ${x.wind_speed_10m} \n Synced at: ${ToShortDateTime(x.timestamp)}`]
    });

    setLowestTempPerCity(tempData);
    setHigestWindspeedPerCity(windData);
  }

  function ToShortDateTime(datetime) {
    var date = datetime.substring(0, 10);
    var time = datetime.substring(11, 19);

    return `${date} ${time}`
  }

  return (
    <>
      {higestWindspeedPerCity.length > 0 &&
        <>
          <h3>
            Lowest temperature
          </h3>
          <Chart
            chartType="ColumnChart"
            data={[["City", "Temperature", { role: "tooltip" }], ...lowestTempPerCity]}
            width="110%"
            height="600px"
            legendToggle
          />
        </>
      }
      {higestWindspeedPerCity.length > 0 &&
        <>
          <h3>
            Highest windspeed
          </h3>
          <Chart
            chartType="ColumnChart"
            data={[["City", "Wind speed", { role: "tooltip" }], ...higestWindspeedPerCity]}
            width="110%"
            height="600px"
            legendToggle
          />
        </>
      }
    </>
  )
}

export default App
