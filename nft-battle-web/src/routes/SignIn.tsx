import { useState } from 'react';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import icon from '../images/icon.png'
import { login } from '../services/api';
import { Snackbar } from '@mui/material';

const theme = createTheme();

export function SignIn() {
  const [name, setName] = useState("");
  const [password, setPassword] = useState("");

  const [isOpenSnackBar, setIsOpenSnackbar] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState("")

  const handleSubmit = async () => {
    try {
      const user = await login(name, password);

    } catch (errorMessage: any) {
      setSnackbarMessage(errorMessage)
      setIsOpenSnackbar(true)
    }

  };

  return (
    <ThemeProvider theme={theme}>
      <Container component="main" maxWidth="xs">
        <CssBaseline />
        <Box sx={{ marginTop: 8, display: 'flex', flexDirection: 'column', alignItems: 'center', }}>
          <img src={icon} style={{ width: 200, height: 200, marginBottom: 20 }} />
          <Typography component="h1" variant="h5">
            NFT Battle
          </Typography>
          <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
            <TextField
              margin="normal"
              required
              onChange={(event) => setName(event.target.value)}
              fullWidth
              label="Nome"
              autoFocus
            />
            <TextField
              margin="normal"
              required
              onChange={(event) => setPassword(event.target.value)}
              fullWidth
              label="Senha"
              type="password"
              autoComplete="current-password"
            />

            <Button fullWidth variant="contained" sx={{ mt: 3, mb: 2 }} onClick={handleSubmit}>
              Entrar
            </Button>
            <Grid container justifyContent="flex-end">
              <Grid item >
                <Link href="/signup" variant="body2">
                  {"Cadastrar"}
                </Link>
              </Grid>
            </Grid>
          </Box>
        </Box>

        <Snackbar
          open={isOpenSnackBar}
          autoHideDuration={6000}
          onClose={() => setIsOpenSnackbar(false)}
          message={snackbarMessage}
        />
      </Container>
    </ThemeProvider>
  );
}