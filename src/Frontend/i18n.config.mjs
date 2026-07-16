import { readFile } from 'node:fs/promises';

async function loadJsonLocale(path) {
  const content = await readFile(new URL(path, import.meta.url), 'utf8');
  return JSON.parse(content);
}

export default defineI18nConfig(async () => {
  const [enMessages, huMessages] = await Promise.all([
    loadJsonLocale('./locales/en.json'),
    loadJsonLocale('./locales/hu.json')
  ]);

  return {
    legacy: false,
    locale: 'en',
    fallbackLocale: 'en',
    messages: {
      en: enMessages,
      hu: huMessages
    }
  };
});
