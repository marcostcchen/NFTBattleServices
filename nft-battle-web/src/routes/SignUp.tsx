import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { useState } from 'react';
import { CircularProgress, Snackbar } from '@mui/material';
import { createUser } from '../services/api';
import { useNavigate } from "react-router-dom";

const theme = createTheme();

export function SignUp() {
  const [name, setName] = useState("");
  const [password, setPassword] = useState("");
  const [walletId, setWalletId] = useState("");

  const [isOpenSnackBar, setIsOpenSnackbar] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState("")

  const [isLoading, setIsLoading] = useState(false);

  const navigate = useNavigate();
  const handleSubmit = async () => {
    try {
      setIsLoading(true)
      const user = await createUser(name.trim().toLocaleLowerCase(), password.trim().toLocaleLowerCase(), walletId.trim().toLocaleLowerCase());
      setSnackbarMessage("Usuário cadastrado com sucesso!");
      setIsOpenSnackbar(true);
      setTimeout(() => {
        navigate('/');
        setIsLoading(false)
      }, 2000)
    } catch (errorMessage: any) {
      setSnackbarMessage(errorMessage)
      setIsOpenSnackbar(true)
      setIsLoading(false)
    }

  };

  return (
    <ThemeProvider theme={theme}>
      <Container component="main" maxWidth="xs">
        <CssBaseline />
        <Box
          sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}
        >
          <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Cadastro
          </Typography>
          <Box component="form" noValidate sx={{ mt: 3 }}>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <TextField
                  required
                  fullWidth
                  label="Nome"
                  onChange={(event) => setName(event.target.value)}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  required
                  fullWidth
                  label="Senha"
                  type="password"
                  onChange={(event) => setPassword(event.target.value)}
                  autoComplete="new-password"
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  required
                  fullWidth
                  label="Wallet"
                  onChange={(event) => setWalletId(event.target.value)}
                />
              </Grid>
            </Grid>
            <Button
              fullWidth
              variant="contained"
              style={{height: 50}}
              sx={{ mt: 3, mb: 2,  }}
              onClick={handleSubmit}
            >
              {isLoading && (
                <CircularProgress color="warning"/>
              )}
              {!isLoading && (
                <>
                  Cadastrar
                </>
              )}
            </Button>
            <Grid container justifyContent="flex-end">
              <Grid item>
                <Link href="/" variant="body2">
                  Entrar
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