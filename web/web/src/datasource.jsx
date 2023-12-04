export async function GetWeatherDataFromApi() {
    var response = await fetch("https://localhost:7087/weather", {
        method: "GET",
    });

    var result = await response.json();
    return result;
}