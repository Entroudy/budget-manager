/**
 * @description get user form localStorage
 */
export const getUser = (): string | null => {
  return window.localStorage.getItem('user')
}

/**
 * @description save user localStorage
 */
export const saveUser = (userData: string): void => {
  window.localStorage.setItem('user', userData)
}

/**
 * @description remove user localStorage
 */
export const removeUser = (): void => {
  window.localStorage.removeItem('user')
}

export default { getUser, saveUser, removeUser }
