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
      display: ['Georgia', '"Times New Roman"', ...defaultTheme.fontFamily.serif],
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
        primary: '#24513F',
        'primary-dark': '#17382D',
        secondary: '#365B49',
        brand: {
          darkBrown: {
            DEFAULT: '#17382D',
            700: '#10291F',
            500: '#214638',
          },
          lightBrown: {
            DEFAULT: '#A47755',
          },
          lightGreen: {
            DEFAULT: '#E5EBDD',
          },
          darkGreen: {
            DEFAULT: '#244B3B',
          },
        },
        success: '#557E58',
        error: '#B85C4C',
        warning: '#D39A51',
        info: '#6F8871',
        dark: {
          DEFAULT: '#17382D',
          text: '#FFFFFF',
        },
        body: '#F6F3E9'
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
