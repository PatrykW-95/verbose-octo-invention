/**
 * Validate a license key using the Fetch API (Node.js 18+ or browser).
 *
 * Usage (Node.js):
 *   node example.js <host> <license-key>
 */

const host = process.argv[2] ?? "https://licenses.example.com";
const key  = process.argv[3] ?? "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

async function validateLicense(host, key) {
  const url = `${host}/api/licensevalidation/validate?key=${encodeURIComponent(key)}`;

  const response = await fetch(url, {
    method: "GET",
    headers: { Accept: "application/json" },
  });

  const data = await response.json();

  if (data.valid) {
    console.log("✅ License is valid");
    console.log("  Type     :", data.licenseType);
    console.log("  Email    :", data.email);
    console.log("  Expires  :", data.expiresAt ?? "never");
  } else {
    console.log("❌ License is not valid:", data.message);
  }

  return data;
}

validateLicense(host, key).catch(console.error);
