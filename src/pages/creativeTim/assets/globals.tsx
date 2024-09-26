import { createGlobalStyle } from "styled-components";

const GlobalStyles = createGlobalStyle`
  html {
    scroll-behavior: smooth;
  }

  *, *::before, *::after {
    margin: 0;
    padding: 0;
  }

  a, a:link, a:visited {
    text-decoration: none !important;
  }

  a.link, .link, a.link:link, .link:link, a.link:visited, .link:visited {
    color: ${({ theme }) => theme.colors.dark.main} !important;
    transition: color 150ms ease-in !important;
  }

  a.link:hover, .link:hover, a.link:focus, .link:focus {
    color: ${({ theme }) => theme.colors.info.main} !important;
  }
`;

export default GlobalStyles;
