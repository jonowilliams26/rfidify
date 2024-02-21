from datetime import datetime
from PiicoDev_RFID import PiicoDev_RFID
from PiicoDev_Unified import sleep_ms
from requests import post
from PiicoDev_Buzzer import PiicoDev_Buzzer


# Contants
# ------------------------------
last_seen_threshold_seconds = 1
scan_url = "http://api:8080/rfids/scan"
max_buzzer_volume = 2
buzz_duration_ms = 100
buzz_frequency_hz = 1000
ok = 200
not_found = 404


# Variables
# ------------------------------
rfid_module = PiicoDev_RFID()
buzzer = PiicoDev_Buzzer()
buzzer.volume(max_buzzer_volume)
last_seen_rfid = None
last_seen_at = None


# Functions
# ------------------------------
def read_rfid() -> str | None:
    """Reads the RFID from the RFID module

    Returns:
        str : The RFID read from the module
        None : If no RFID is present, failed to read the RFID, or the RFID has been scanned within the last last_seen_threshold_seconds
   """
    tag_present = rfid_module.tagPresent();

    if not tag_present:
        return None

    result = rfid_module.readID(detail=True)

    if not result['success']:
        print("Failed to read RFID tag")
        return None

    rfid = result['id_formatted']
    print(f'Successfully read RFID: {rfid} from RFID module')

    global last_seen_rfid
    global last_seen_at
    below_last_seen_threshold = last_seen_at is not None and (datetime.now() - last_seen_at).total_seconds() < last_seen_threshold_seconds

    if rfid == last_seen_rfid and below_last_seen_threshold:
        print(f'RFID: {rfid} already scanned within the last {last_seen_threshold_seconds} seconds. Skipping.')
        return None

    last_seen_rfid = rfid
    last_seen_at = datetime.now()
    return rfid


def send_rfid(rfid: str | None):
    """
    Sends the RFID to the API

    Args:
        rfid (str | None): The RFID to send to the API, if None, the function will return without sending the RFID
    """
    if rfid is None:
        return

    print(f'Sending RFID: {rfid}')
    data = { "id": rfid }

    try:
        response = post(scan_url, json=data)
        if response.status_code == ok or response.status_code == not_found:
            print(f'Successfully sent RFID: {rfid}')
            beep()
        else:
            response.raise_for_status()
    except:
        print(f'An unexpected error occurred trying to send RFID: {rfid}')
        beep(3)

def beep(amount: int = 1):
    for x in range(amount):
        buzzer.tone(buzz_frequency_hz, buzz_duration_ms)
        sleep_ms(250)

# Main
print("Starting RFID scanner...")
while True:
    rfid = read_rfid()
    send_rfid(rfid)
    sleep_ms(100)