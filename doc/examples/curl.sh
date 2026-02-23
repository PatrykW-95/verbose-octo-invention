#!/usr/bin/env bash
# Validate a license key using cURL.
# Usage: ./curl.sh <host> <license-key>
#   <host>        Base URL of the License Manager service, e.g. https://licenses.example.com
#   <license-key> The GUID license key to validate

HOST="${1:?Usage: $0 <host> <license-key>}"
KEY="${2:?Usage: $0 <host> <license-key>}"

curl -s -X GET \
  "${HOST}/api/licensevalidation/validate?key=${KEY}" \
  -H "Accept: application/json" | jq .
