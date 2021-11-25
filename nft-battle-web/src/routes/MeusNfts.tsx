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
import { Nft } from '../models';
import { getUserNfts } from '../services/api';

export function MeusNfts() {
  const [isOpenSnackBar, setIsOpenSnackbar] = useState(false);
  const [isBuyModalOpen, setIsBuyModalOpen] = useState(false);

  const [isLoading, setIsLoading] = useState(false);
  const [buyIdNft, setBuyIdNft] = useState("");
  const [nfts, setNfts] = useState<Array<Nft>>([])
  const [snackbarMessage, setSnackbarMessage] = useState("")

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

  return (
    <PersistentDrawerLeft screenName="Meus NFTs">
      <>
        <Typography paragraph>
          Total de Nfts: {nfts.length}
        </Typography>
        <Container sx={{ py: 6 }} maxWidth="md">
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
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Container>
      </>
    </PersistentDrawerLeft>
  )
}
