"""
Validate a license key using the Python `requests` library.

Usage:
    pip install requests
    python example.py <host> <license-key>
"""

import sys
import requests

host = sys.argv[1] if len(sys.argv) > 1 else "https://licenses.example.com"
key  = sys.argv[2] if len(sys.argv) > 2 else "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"

url = f"{host}/api/licensevalidation/validate"
response = requests.get(url, params={"key": key}, timeout=10)

data = response.json()

if data.get("valid"):
    print("✅ License is valid")
    print(f"  Type    : {data['licenseType']}")
    print(f"  Email   : {data['email']}")
    print(f"  Expires : {data.get('expiresAt') or 'never'}")
else:
    print(f"❌ License is not valid: {data['message']}")
