# rfidify
A Raspberry PI RFID reader that plays things on Spotify

## Hardware
- [Raspberry PI 5](https://www.raspberrypi.com/products/raspberry-pi-5/)
- [Piicodev RFID Module](https://core-electronics.com.au/piicodev-rfid-module.html)
- [Piicodev Adapter for Raspberry PI](https://core-electronics.com.au/piicodev-adapter-for-raspberry-pi.html)
- [Piicodev Cable](https://core-electronics.com.au/piicodev/cables.html)
- [RFID Tags](https://core-electronics.com.au/catalogsearch/result/?order=bestsellers&q=ntag213)

## Software
### RFID Reader
- [Python](https://www.python.org/)
- [Piicodev Python Library](https://github.com/CoreElectronics/CE-PiicoDev-PyPI)

### Frontend
- [Sveltekit](https://kit.svelte.dev/)
- [Shadcn Svelte UI Library](https://www.shadcn-svelte.com/)

### Backend
- [.NET 8 Minimal API](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-8.0)
- [NGINX as a reverse proxy](https://www.nginx.com/)
- [SQLite](https://www.sqlite.org/index.html)

## How to Run on your Raspberry PI
1. Enabled the `IC2` interface on your Raspberry PI
    - Open the Raspberry Pi Configuration Menu
    - Select the Interfaces tab
    - Ensure `I2C` is enabled.
3. Install `git` and `docker` on your Raspberry PI
4. On your Raspberry PI, run `hostname -I` and note down your `IP Address`
5. Go to the [Spotify Web API docs](https://developer.spotify.com/documentation/web-api) and create an `app`
6. Enter `http://<your raspberry pi IP>/spotify/setup/callback` as the `redirect URI` in the Spotify app settings
7. On your Raspberry PI, `clone` the repository and  run `cd rfidify`
8. On your Raspberry PI, run `docker compose up` to start the docker containers
9. Open a web browser and go to `http://<your raspberry pi IP>`
10. You will be prompted to enter your Spotify `client id` and `secret` which was created in `step 4`
11. You app is setup and ready to run!

## Architecture
![image](https://github.com/jonathanjameswilliams26/rfidify/assets/37890156/6dee1003-a9ee-4d06-89d3-7d280fb2114c)


