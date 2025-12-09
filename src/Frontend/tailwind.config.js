import defaultTheme from 'tailwindcss/defaultTheme'

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './components/**/*.{js,vue,ts}',
    './layouts/**/*.vue',
    './pages/**/*.vue',
    './plugins/**/*.{js,ts}',
    './nuxt.config.{js,ts}',
    './app.vue',
  ],
  darkMode: 'selector',
  theme: {
    fontFamily: {
      maven: ['"Maven Pro"', ...defaultTheme.fontFamily.sans],
    },
    extend: {
      spacing: {
        '262px': '262px',
      },
      screens: {
        xs: '400px',
        print: { raw: 'print' },
        screen: { raw: 'screen' },
      },
      minWidth: {
        '200px': '200px',
        '75px': '75px',
      },
      width: {
        '460px': '460px',
      },
      height: {
        '88px': '88px',
      },
      minHeight: {
        '70px': '70px',
        '50vh': '50vh',
      },
      maxWidth: {
        2.5: '696px',
        '90p': '90%',
      },
      borderWidth: {
        0.5: '0.5px',
      },
      margin: {
        '70px': '70px',
        '96px': '96px',
        '288px': '288px',
        '256px': '256px',
        '260px': '260px',
        '208px': '208px',
      },
      colors: {
        primary: '#442D1C',
        secondary: '#6B4E3D',
        brand: {
          darkBrown: {
            DEFAULT: '#442D1C',
            700: '#352415',
            500: '#2A1D11',
          },
          lightBrown: {
            DEFAULT: '#8B6F47',
          },
          lightGreen: {
            DEFAULT: '#DFF8D5',
          },
          darkGreen: {
            DEFAULT: '#1e2e39',
          },
        },
        success: '#56c45d',
        error: '#ee4444',
        warning: '#C8D414',
        info: '#625ae3',
        dark: {
          DEFAULT: '#442D1C',
          text: '#FFFFFF',
        },
        body: '#f1f5f9'
      },
    },
    variants: {
      extend: {
        backgroundColor: ['dark'],
        textColor: ['dark'],
      },
    },
  },
  plugins: [],
}
