from datetime import datetime
from PiicoDev_RFID import PiicoDev_RFID
from PiicoDev_Unified import sleep_ms
from requests import post

print("Starting RFID scanner...")
rfid_module = PiicoDev_RFID()
url = "http://api:8080/rfids/scan"
last_seen_rfid = None
last_seen_at = None
last_seen_threshold_seconds = 1

print('Place tag near the PiicoDev RFID Module')

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
        sleep_ms(100)
        continue

    print(f'Sending RFID: {rfid}')
    last_seen_rfid = rfid
    last_seen_at = datetime.now()
    data = { "id": rfid }

    response = post(url, json=data)
    if response.status_code == 200:
        print(f'Successfully started playing RFID: {rfid}')
    elif response.status_code == 404:
        print(f'RFID: {rfid} not found')
    else:
        print(f'Failed to start playing RFID: {rfid}')

    sleep_ms(100)