/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{js,jsx,ts,tsx}"],
  plugins: [require("tailwindcss-rtl")],
  theme: {
    extend: {
      boxShadow: {
        "3xl": "0 6px 60px 0px rgba(0, 0, 0, 0.3)",
      },
    },
  },
};
