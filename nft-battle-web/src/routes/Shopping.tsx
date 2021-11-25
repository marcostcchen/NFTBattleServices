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
import { getNfts } from '../services/api';
import { Snackbar } from '@mui/material';
import { Nft } from '../models';

export function Shopping() {
  const [isOpenSnackBar, setIsOpenSnackbar] = useState(false);
  const [nfts, setNfts] = useState<Array<Nft>>([])
  const [snackbarMessage, setSnackbarMessage] = useState("")

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

  return (
    <PersistentDrawerLeft screenName="Shopping">
      <main>
        <Box sx={{ bgcolor: 'background.paper', pt: 6, pb: 2, }}>
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
        <Container sx={{ py: 8 }} maxWidth="md">
          <Grid container spacing={4}>
            {nfts.map((nft) => (
              <Grid item key={nft.id} xs={12} sm={8} md={4}>
                <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }} >
                  <CardMedia
                    style={{ width: "210px", height: "300px", margin: '0 auto 0' }}
                    component="img"
                    image={nft.imageUrl}
                    alt="random"
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
                      Tipo: {nft.type}
                    </Typography>
                    <Typography>
                      IdDono: {nft.idOwner ?? " - "}
                    </Typography>
                  </CardContent>
                  <CardActions>
                    {nft.idOwner == null ?
                      (<Button size="small">Comprar</Button>)
                      :
                      (<Button size="small">Negociar</Button>)}
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
      </main>
    </PersistentDrawerLeft>
  );
}