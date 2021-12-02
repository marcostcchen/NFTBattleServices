import Button from '@mui/material/Button';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import PersistentDrawerLeft from '../components/PersistentDrawerLeft';
import { useEffect, useState } from 'react';
import { fetchBuyNft, getNfts } from '../services/api';
import { Snackbar, Backdrop, CircularProgress } from '@mui/material';
import Modal from '@mui/material/Modal';
import { Nft, Owner } from '../models';

export function Shopping() {
  const [isOpenSnackBar, setIsOpenSnackbar] = useState(false);
  const [isBuyModalOpen, setIsBuyModalOpen] = useState(false);
  const [isShowNegociateModal, setIsShowNegociateModal] = useState(false);

  const [isLoading, setIsLoading] = useState(false);
  const [buyIdNft, setBuyIdNft] = useState("");
  const [nfts, setNfts] = useState<Array<Nft>>([])
  const [snackbarMessage, setSnackbarMessage] = useState("")

  const [negociateOwner, setNegociateOwner] = useState<Owner>({ id: "1234", name: " - " })

  useEffect(() => {
    fetchNfts();
  }, [])

  const fetchNfts = async () => {
    try {
      const nfts = await getNfts();
      setNfts(nfts)
    } catch (errorMessage: any) {
      setSnackbarMessage(errorMessage)
      setIsOpenSnackbar(true)
    }
  }

  const showConfirmBuyNft = (idNft: string) => {
    setBuyIdNft(idNft);
    setIsBuyModalOpen(true)
  }

  const buyNft = async () => {
    setIsBuyModalOpen(false);
    setIsLoading(true)
    try {
      const nft = await fetchBuyNft(buyIdNft);
      setIsLoading(false);
      setSnackbarMessage("NFT comprado com sucesso!")
      setIsOpenSnackbar(true)
      await fetchNfts()
    } catch (errorMessage: any) {
      setIsLoading(false);
      setSnackbarMessage(errorMessage)
      setIsOpenSnackbar(true)
    }
  }

  const showNegociateModal = (owner: Owner) => {
    setNegociateOwner(owner);
    setIsShowNegociateModal(true);
  }

  return (
    <>
      <PersistentDrawerLeft screenName="Loja NFT">
        <main>
          <Box sx={{ bgcolor: 'background.paper', pt: 4, pb: 0, }}>
            <Container maxWidth="sm">
              <Typography
                component="h1"
                variant="h2"
                align="center"
                color="text.primary"
                gutterBottom
              >
                Loja de NFT
              </Typography>
              <Typography variant="h5" align="center" color="text.secondary" paragraph>
                Conquiste os NFTs de seus sonhos para batalhar contra os seus adversários
              </Typography>
            </Container>
          </Box>
          <Container sx={{ py: 6 }} maxWidth="md">
            <Grid container spacing={4}>
              {nfts.length == 0 && (
                <div style={{width: '100%', alignItems:'center', justifyContent:'center'}}>
                  <CircularProgress />
                </div>
              )}
              {nfts.map((nft) => (
                <Grid item key={nft.id} xs={12} sm={8} md={4}>
                  <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }} >
                    <CardMedia
                      style={{ width: "210px", height: "300px", margin: '0 auto 0' }}
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
                      <Typography>
                        Dono: {nft.owner?.name ?? " - "}
                      </Typography>
                    </CardContent>
                    <CardActions>
                      {nft.owner ?
                        (<Button size="small" onClick={() => showNegociateModal(nft.owner)}>Negociar</Button>)
                        :
                        (<Button size="small" onClick={() => showConfirmBuyNft(nft.id)}>Comprar</Button>)}
                    </CardActions>
                  </Card>
                </Grid>
              ))}
            </Grid>
          </Container>

          <Snackbar
            open={isOpenSnackBar}
            autoHideDuration={6000}
            onClose={() => setIsOpenSnackbar(false)}
            message={snackbarMessage}
          />

          <Modal
            open={isBuyModalOpen}
            onClose={() => setIsBuyModalOpen(false)}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
          >
            <Box sx={modalStyle}>
              <Typography id="modal-modal-title" variant="h6" component="h2">
                Confirma Comprar este Nft?
              </Typography>
              <Button onClick={buyNft}>Sim</Button>
              <Button onClick={() => setIsBuyModalOpen(false)}>Não</Button>
            </Box>
          </Modal>

          <Modal
            open={isShowNegociateModal}
            onClose={() => setIsShowNegociateModal(false)}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
          >
            <Box sx={modalStyle}>
              <Typography id="modal-modal-title" variant="h6" component="h3" style={{ marginBottom: 10 }}>
                Entre em contato com o dono para negociar. Em seguida o dono poderá transferir para você.
              </Typography>
              <Typography>
                Nome: {negociateOwner?.name ?? " - "}
              </Typography>
              <Typography>
                Telefone: "(11) 1234-5678"
              </Typography>
            </Box>
          </Modal>

        </main>
      </PersistentDrawerLeft>
      <Backdrop
        sx={{ color: '#fff' }}
        open={isLoading}
        onClick={() => setIsLoading(false)}
      >
        <div style={{ textAlign: 'center' }}>
          <CircularProgress color="inherit" />
          <p>Efetuando a compra...</p>
        </div>
      </Backdrop>
    </>
  );
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