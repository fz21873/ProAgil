import { Lote } from './Lote';
import { RedeSocial } from './RedeSocial';
import { Palestrante } from './Palestrante';

export interface Evento{
     id: number ;
     tema: string ;
     local: string ;
     dataEvento: Date;
     qtdPessoas: number;
     imagemURL: string ;
     telefone: string ;
     email: string ;
     lotes: Lote[];
     redesSociais: RedeSocial[];
     palestrantesEventos: Palestrante[];

}
