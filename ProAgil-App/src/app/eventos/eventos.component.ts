import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_model/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal/';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { defineLocale, ptBrLocale} from 'ngx-bootstrap/chronos';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
defineLocale('pt-br', ptBrLocale);




@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
})
export class EventosComponent implements OnInit {
  titulo = 'Eventos';
  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  modoSalvar = 'post';
  imagemLargura = 50;
  imagemMargin = 2;
  mostrarImagem = false;
  registerForm: FormGroup;
  bodyDeletarEvento = '';
  file: File;
  fileNameToUpdate: string;
  dataAtual: string;
  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService
    ) {
      this.localeService.use('pt-br');
    }

    // tslint:disable-next-line:variable-name
    _filtroLista: string;
    get filtroLista(): string{

      return this._filtroLista;
    }

    set filtroLista(value: string){

      this._filtroLista = value;
      this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
    }

    openModal(template: any): void{
      this.registerForm.reset();
      template.show();
    }

    onFileChange(event): void{
      const reader = new FileReader();

      if (event.target.files && event.target.files.length)
      {
        this.file = event.target.files;
        console.log(this.file);
      }
    }

    uploadImagem(): void{

      if (this.modoSalvar === 'post'){
      const nomeArquivo = this.evento.imagemURL.split('\\', 3);
      this.evento.imagemURL = nomeArquivo[2];
      this.eventoService.postUpload(this.file, nomeArquivo[2]).subscribe(
        () => {
          this.dataAtual = new Date().getMilliseconds().toString();
          this.getEventos();
        }
      );
      }else{
        this.evento.imagemURL = this.fileNameToUpdate;
        this.eventoService.postUpload(this.file, this.fileNameToUpdate).subscribe(
          () => {
            this.dataAtual = new Date().getMilliseconds().toString();
            this.getEventos();
          }
        );
      }
    }
    editarEvento(evento: Evento, template: any): void {
      this.modoSalvar = 'put';
      this.openModal(template);
      this.evento = Object.assign({}, evento);
      this.fileNameToUpdate = evento.imagemURL.toString();
      this.evento.imagemURL = '';
      this.registerForm.patchValue(this.evento);
    }

    novoEvento(template: any): void{
      this.modoSalvar = 'post';
      this.openModal(template);
    }

    excluirEvento(evento: Evento, template: any): void{
      this.openModal(template);
      this.evento = evento;
      this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
    }

    confirmeDelete(template: any): void{
      this.eventoService.deleteEvento(this.evento.id).subscribe(
        () => {
          template.hide();
          this.getEventos();
          this.toastr.success('Deletado com Sucesso!');
        }, error => {
          this.toastr.error(`Error ao tentar Deletar: ${error}`);
        }
        );
      }

    // tslint:disable-next-line:typedef
    ngOnInit(){
      this.validation();
      this.getEventos(); }

      alternarImagem(): void { this.mostrarImagem = !this.mostrarImagem; }

      validation(): void{
        this.registerForm = this.fb.group({
          tema:  ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
          local: ['', Validators.required],
          dataEvento: ['', Validators.required],
          qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
          imagemURL: ['', Validators.required],
          telefone: ['', Validators.required],
          email: ['', [Validators.required, Validators.email]]
        });
      }
      salvarAlteracao(template: any): void{

        if (this.registerForm.valid) {
          if (this.modoSalvar === 'post'){
            this.evento = Object.assign({}, this.registerForm.value);
            this.uploadImagem();
            this.eventoService.postEvento(this.evento).subscribe(
              (novoEvento: Evento) => {
                template.hide();
                this.getEventos();
                this.toastr.success('Inserido com Sucesso!');
              }, error => {
                this.toastr.error(`Error ao Inserir:${error}`);
               // console.log(error);
              }
            );

          } else {

            this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);
            this.uploadImagem();
            this.eventoService.putEvento(this.evento).subscribe(
              () => {
                template.hide();
                this.getEventos();
                this.toastr.success('Editado com Sucesso!');
              }, error => {
                this.toastr.error(`Error ao Editar:${error}`);
               // console.log(error);
              }
            );

          }
        }
      }
      getEventos(): void  {

        this.dataAtual = new Date().getMilliseconds().toString();
        this.eventoService.getAllEvento().subscribe(
          (_EVENTOS: Evento[] ) => {
            this.eventos =  _EVENTOS;
            this.eventosFiltrados = this.eventos;
            console.log(_EVENTOS);
          }, error => {
            console.log(error);
          });
        }

        filtrarEventos(filtarPor: string): Evento[] {

          filtarPor = filtarPor.toLocaleLowerCase();
          return this.eventos.filter(
            evento => (evento.tema.toLocaleLowerCase().indexOf(filtarPor) !== -1)
            );
          }

        }
