import PersistentDrawerLeft from '../components/PersistentDrawerLeft'
import Button from '@mui/material/Button';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { useEffect, useState } from 'react';
import { Nft, User } from '../models';
import { fetchSellNft, fetchTransferNft, getUserNfts, getUsers } from '../services/api';
import { Backdrop, CircularProgress, ListItemButton, ListItemText, Modal, Snackbar } from '@mui/material';

export function MeusNfts() {
  const [isOpenSnackBar, setIsOpenSnackbar] = useState(false);
  const [isSellModalOpen, setIsSellModalOpen] = useState(false);
  const [isTransferModalOpen, setIsTransferModalOpen] = useState(false);

  const [isLoading, setIsLoading] = useState(false);
  const [nfts, setNfts] = useState<Array<Nft>>([]);
  const [users, setUsers] = useState<Array<User>>([]);
  const [snackbarMessage, setSnackbarMessage] = useState("")

  const [idNftToSell, setIdNftToSell] = useState("");
  const [idNftToTransfer, setIdNftToTransfer] = useState("");

  const [loadingText, setLoadingText] = useState("");

  useEffect(() => {
    fetchNfts();
  }, [])

  const fetchNfts = async () => {
    try {
      const nfts = await getUserNfts();
      setNfts(nfts)
    } catch (errorMessage: any) {
      setSnackbarMessage(errorMessage)
      setIsOpenSnackbar(true)
    }
  }

  const showTransferModal = async (idNft: string) => {
    setIsLoading(true);
    setLoadingText("Carregando usuários...")
    setIdNftToTransfer(idNft)

    const users = await getUsers(true);
    setUsers(users);

    setIsLoading(false);
    setIsTransferModalOpen(true);
  }

  const showConfirmSellModal = (idNft: string) => {
    setIdNftToSell(idNft);
    setIsSellModalOpen(true);
  }

  const sellNft = async () => {
    setIsLoading(true);
    setIsSellModalOpen(false);
    setLoadingText("Vendendo NFT...")

    try {
      const nft = await fetchSellNft(idNftToSell);

      setIsLoading(false);
      setSnackbarMessage("NFT vendido com sucesso!")
      setIsOpenSnackbar(true);
      fetchNfts();
    } catch (errorMessage: any) {
      setSnackbarMessage(errorMessage)
      setIsOpenSnackbar(true)
    }
  }

  const transferToUser = async (idTransferUser: string) => {
    setIsLoading(true);
    setLoadingText("Tranferindo NFT...");
    setIsTransferModalOpen(false);

    try {
      const nft = await fetchTransferNft(idNftToTransfer, idTransferUser);
      setIsLoading(false);
      setSnackbarMessage("NFT Transferido com sucesso!")
      setIsOpenSnackbar(true)
      fetchNfts();
    } catch (errorMessage: any) {
      setSnackbarMessage(errorMessage)
      setIsOpenSnackbar(true)
    }

  }

  return (
    <PersistentDrawerLeft screenName="Meus NFTs">
      <>
        <Typography paragraph>
          Total de Nfts: {nfts.length}
        </Typography>
        <Container sx={{ py: 6 }} maxWidth="md">
          <Grid container spacing={4}>
            {nfts.length == 0 && (
              <CircularProgress />
            )}

            {nfts.map((nft) => (
              <Grid item key={nft.id} xs={12} sm={8} md={4}>
                <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }} >
                  <CardMedia
                    style={{ width: "300px", height: "300px", margin: '0 auto 0' }}
                    component="img"
                    image={nft.image_url}
                    alt="Sem Imagem"
                  />
                  <CardContent sx={{ flexGrow: 1 }}>
                    <Typography gutterBottom variant="h5" component="h2" style={{ fontSize: 16 }}>
                      Id: {nft.id}
                    </Typography>
                    <Typography>
                      Vida: {nft.health}
                    </Typography>
                    <Typography>
                      Ataque: {nft.attack}
                    </Typography>
                    <Typography>
                      Defesa: {nft.defence}
                    </Typography>
                  </CardContent>
                  <CardActions>
                    <Button size="small" onClick={() => showTransferModal(nft.id)}>Transferir</Button>
                    <Button size="small" onClick={() => showConfirmSellModal(nft.id)}>Vender para Loja</Button>
                  </CardActions>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Container>

        <Modal
          open={isTransferModalOpen}
          onClose={() => setIsTransferModalOpen(false)}
          aria-labelledby="modal-modal-title"
          aria-describedby="modal-modal-description"
        >
          <Box sx={modalStyle}>
            <Typography id="modal-modal-title" variant="h6" component="h2">
              Selecione quem deseja transferir o NFT
            </Typography>

            {users.length == 0 && (
              <p>Nao há usuários</p>
            )}

            {users.map((user) => (
              <ListItemButton key={user.id} component="a" href="#simple-list" onClick={() => transferToUser(user.id)}>
                <ListItemText primary={user.name} />
              </ListItemButton>
            ))}
          </Box>
        </Modal>

        <Modal
          open={isSellModalOpen}
          onClose={() => setIsSellModalOpen(false)}
          aria-labelledby="modal-modal-title"
          aria-describedby="modal-modal-description"
        >
          <Box sx={modalStyle}>
            <Typography id="modal-modal-title" variant="h6" component="h2">
              Confirma Vender o NFT para a loja?
            </Typography>
            <Button onClick={sellNft}>Sim</Button>
            <Button onClick={() => setIsSellModalOpen(false)}>Não</Button>
          </Box>
        </Modal>

        <Snackbar
          open={isOpenSnackBar}
          autoHideDuration={6000}
          onClose={() => setIsOpenSnackbar(false)}
          message={snackbarMessage}
        />

        <Backdrop
          sx={{ color: '#fff' }}
          open={isLoading}
          onClick={() => setIsLoading(false)}
        >
          <div style={{ textAlign: 'center' }}>
            <CircularProgress color="inherit" />
            <p>{loadingText}</p>
          </div>
        </Backdrop>
      </>
    </PersistentDrawerLeft>
  )
}

const modalStyle = {
  position: 'absolute' as 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 400,
  bgcolor: 'background.paper',
  border: '2px solid #000',
  boxShadow: 24,
  p: 4,
}