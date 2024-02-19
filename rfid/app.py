from datetime import datetime
from PiicoDev_RFID import PiicoDev_RFID
from PiicoDev_Unified import sleep_ms
from requests import post
from PiicoDev_Buzzer import PiicoDev_Buzzer

# Constants
last_seen_threshold_seconds = 1
url = "http://api:8080/rfids/scan"
max_buzzer_volume = 2
buzz_duration_ms = 100
buzz_frequency_hz = 1000
ok = 200
not_found = 404

rfid_module = PiicoDev_RFID()
buzzer = PiicoDev_Buzzer()
buzzer.volume(max_buzzer_volume)
last_seen_rfid = None
last_seen_at = None

def beep():
    buzzer.tone(buzz_frequency_hz, buzz_duration_ms)

print("Starting RFID scanner...")
while True:
    tag_present = rfid_module.tagPresent();

    if not tag_present:
        sleep_ms(100)
        continue

    result = rfid_module.readID(detail=True)

    if not result['success']:
        print("Failed to read RFID tag")
        continue

    rfid = result['id_formatted']

    below_last_seen_threshold = last_seen_at is not None and (datetime.now() - last_seen_at).total_seconds() < last_seen_threshold_seconds

    if rfid == last_seen_rfid and below_last_seen_threshold:
        print(f'RFID: {rfid} already scanned within the last {last_seen_threshold_seconds} seconds. Skipping.')
        continue

    print(f'Sending RFID: {rfid}')
    last_seen_rfid = rfid
    last_seen_at = datetime.now()
    data = { "id": rfid }
    response = post(url, json=data)

    if response.status_code == ok:
        print(f'Successfully started playing RFID: {rfid}')
        beep()
    elif response.status_code == not_found:
        print(f'RFID: {rfid} not found')
        beep()
    else:
        print(f'An unexpected error occurred trying to send RFID: {rfid} response: {response}')
    
    sleep_ms(100)