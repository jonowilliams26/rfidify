from datetime import datetime
from PiicoDev_RFID import PiicoDev_RFID
from PiicoDev_Unified import sleep_ms
from requests import post
from PiicoDev_Buzzer import PiicoDev_Buzzer

rfid_module = PiicoDev_RFID()
buzzer = PiicoDev_Buzzer()
url = "http://api:8080/rfids/scan"
last_seen_rfid = None
last_seen_at = None
last_seen_threshold_seconds = 1

def main():
    print("Starting RFID scanner...")
    while True:
        scan_rfid()
        sleep_ms(100)

def scan_rfid():
    tag_present = rfid_module.tagPresent();
    
    if not tag_present:
        sleep_ms(100)
        return

    result = rfid_module.readID(detail=True)

    if not result['success']:
        print("Failed to read RFID tag")
        return

    rfid = result['id_formatted']

    below_last_seen_threshold = last_seen_at is not None and (datetime.now() - last_seen_at).total_seconds() < last_seen_threshold_seconds

    if rfid == last_seen_rfid and below_last_seen_threshold:
        print(f'RFID: {rfid} already scanned within the last {last_seen_threshold_seconds} seconds. Skipping.')
        return

    print(f'Sending RFID: {rfid}')
    last_seen_rfid = rfid
    last_seen_at = datetime.now()
    data = { "id": rfid }

    response = post(url, json=data)
    if response.status_code == 200:
        print(f'Successfully started playing RFID: {rfid}')
        buzz()
    elif response.status_code == 404:
        print(f'RFID: {rfid} not found')
        buzz()
    else:
        print(f'Failed to start playing RFID: {rfid}')

def buzz():
    buzzer.tone(800, 500)

if __name__ == '__main__':
    main()