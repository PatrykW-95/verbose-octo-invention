# License Validation API

This document describes how clients can validate their license keys using the License Manager validation API.

---

## Endpoint

```
GET /api/licensevalidation/validate
```

### Base URL

Replace `<host>` with the domain or IP address where the License Manager service is hosted.

```
https://<host>/api/licensevalidation/validate
```

---

## Request

### Query Parameters

| Parameter | Type   | Required | Description                          |
|-----------|--------|----------|--------------------------------------|
| `key`     | string | Yes      | The license key (GUID) to validate.  |

### Example Request

```
GET https://<host>/api/licensevalidation/validate?key=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```

---

## Responses

All responses are returned as JSON.

### 200 OK — Valid License

Returned when the license key exists, is not revoked, and has not expired.

```json
{
  "valid": true,
  "message": "License is valid.",
  "licenseType": "Free",
  "email": "user@example.com",
  "expiresAt": "2026-12-31T23:59:59"
}
```

| Field         | Type            | Description                                            |
|---------------|-----------------|--------------------------------------------------------|
| `valid`       | boolean         | `true` when the license is valid.                      |
| `message`     | string          | Human-readable status message.                         |
| `licenseType` | string          | License tier: `"Free"` or `"Paid"`.                    |
| `email`       | string          | Email address associated with the license.             |
| `expiresAt`   | string / `null` | ISO 8601 expiry date, or `null` if it does not expire. |

---

### 200 OK — Revoked License

Returned when the license has been administratively revoked.

```json
{
  "valid": false,
  "message": "License has been revoked.",
  "licenseType": "Paid"
}
```

---

### 200 OK — Expired License

Returned when the license has passed its expiry date.

```json
{
  "valid": false,
  "message": "License has expired.",
  "licenseType": "Free"
}
```

---

### 400 Bad Request — Missing Key

Returned when the `key` query parameter is absent or blank.

```json
{
  "valid": false,
  "message": "License key is required."
}
```

---

### 404 Not Found — Unknown Key

Returned when the provided key does not exist in the system.

```json
{
  "valid": false,
  "message": "License key not found."
}
```

---

## Response Field Reference

| Field         | Present when         | Type            |
|---------------|----------------------|-----------------|
| `valid`       | Always               | boolean         |
| `message`     | Always               | string          |
| `licenseType` | Key found in system  | string          |
| `email`       | License is valid     | string          |
| `expiresAt`   | License is valid     | string or null  |

---

## Integration Examples

See the [`examples/`](examples/) folder for ready-to-use code snippets:

- [`curl.sh`](examples/curl.sh) — cURL (command line)
- [`example.js`](examples/example.js) — JavaScript (fetch)
- [`example.py`](examples/example.py) — Python (requests)
- [`example.cs`](examples/example.cs) — C# (HttpClient)

---

## Notes

- License keys are **GUIDs** (e.g. `3fa85f64-5717-4562-b3fc-2c963f66afa6`).
- All timestamps use **UTC** and are formatted as ISO 8601.
- Always check the `valid` field first; do not rely solely on the HTTP status code to determine validity.
